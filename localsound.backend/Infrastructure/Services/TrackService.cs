using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Helper;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class TrackService : ITrackService
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITrackRepository _trackRepository;
        private readonly BlobStorageSettingsAdaptor _blobStorageSettings;
        private readonly ILogger<TrackService> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchHelper _searchHelper;

        public TrackService(IBlobRepository blobRepository, IAccountRepository accountRepository, ITrackRepository trackRepository, BlobStorageSettingsAdaptor blobStorageSettings, ILogger<TrackService> logger, IMapper mapper, ISearchHelper searchHelper)
        {
            _blobRepository = blobRepository;
            _accountRepository = accountRepository;
            _trackRepository = trackRepository;
            _blobStorageSettings = blobStorageSettings;
            _logger = logger;
            _mapper = mapper;
            _searchHelper = searchHelper;
        }

        public async Task<ServiceResponse> DeleteArtistTrackAsync(Guid userId, string memberId, Guid trackId)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your track, please try again..."
                    };
                }

                var track = await _trackRepository.GetArtistTrackAsync(memberId, trackId);

                if (track is null || track.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound)
                    {
                        ServiceResponseMessage = "An error occured deleting your track, please try again..."
                    };
                }

                if (track.ReturnData.TrackImage != null)
                {
                    var imageDeleteResult = await _blobRepository.DeleteBlobAsync(track.ReturnData.TrackImage.FileLocation);

                    if (imageDeleteResult is null || !imageDeleteResult.IsSuccessStatusCode)
                    {
                        // Push delete operation to a queue so it can be done at a different time
                    }
                }

                var trackDeleteResult = await _blobRepository.DeleteBlobAsync(track.ReturnData.TrackData.FileLocation);

                if (trackDeleteResult is null || !trackDeleteResult.IsSuccessStatusCode)
                {
                    // Push delete operation to a queue so it can be done at a different time
                }

                var dbDeleteResult = await _trackRepository.DeleteTrackAsync(track.ReturnData);

                if (dbDeleteResult is null || !dbDeleteResult.IsSuccessStatusCode)
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your track, please try again..."
                    };

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(DeleteArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured deleting your track, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<TrackUploadSASDto>> GenerateTrackUploadSASDtoAsync(Guid userId, string memberId)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError);
                }

                BlobContainerClient container = new (_blobStorageSettings.ConnectionString, userId.ToString());

                if (!await container.ExistsAsync())
                {
                    await container.CreateIfNotExistsAsync();
                    await container.SetAccessPolicyAsync(PublicAccessType.Blob);
                }

                if (!container.CanGenerateSasUri)
                    return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError, "The container can't generate SAS URI");

                var sasBuilder = new BlobSasBuilder
                {
                    BlobContainerName = container.Name,
                    Resource = "c",
                    ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(_blobStorageSettings.TokenExpirationMinutes)
                };

                sasBuilder.SetPermissions(BlobContainerSasPermissions.All);

                var sasUri = container.GenerateSasUri(sasBuilder);

                var trackId = Guid.NewGuid();

                var result = new TrackUploadSASDto
                {
                    AccountName = container.AccountName,
                    AccountUrl = $"{container.Uri.Scheme}://{container.Uri.Host}",
                    ContainerName = userId.ToString(),
                    TrackId = trackId,
                    UploadLocation = $"uploads/{trackId}",
                    SasUri = sasUri,
                    SasToken = sasUri.Query.TrimStart('?'),
                };

                return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.OK)
                {
                    ReturnData = result
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(GenerateTrackUploadSASDtoAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<ArtistTrackUploadDto>> GetArtistTrackAsync(string memberId, Guid trackId)
        {
            try
            {
                var track = await _trackRepository.GetArtistTrackAsync(memberId, trackId);

                var returnData = _mapper.Map<ArtistTrackUploadDto>(track.ReturnData);

                return new ServiceResponse<ArtistTrackUploadDto>(HttpStatusCode.OK)
                { 
                    ReturnData = returnData 
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(GetArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<ArtistTrackUploadDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured retrieving track details, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<TrackListResponseDto>> GetArtistTracksAsync(Guid? userId, string memberId, int page)
        {
            try
            {
                var tracksResult = await _trackRepository.GetArtistTracksAsync(memberId, page);

                if (!tracksResult.IsSuccessStatusCode || tracksResult.ReturnData is null)
                {
                    return new ServiceResponse<TrackListResponseDto>(tracksResult.StatusCode);
                }

                var trackList = _mapper.Map<List<ArtistTrackUploadDto>>(tracksResult.ReturnData);

                if (userId is not null)
                {
                    var songIds = await _trackRepository.GetLikedSongsIdsAsync(userId);

                    if (songIds.IsSuccessStatusCode && songIds.ReturnData is not null && songIds.ReturnData.Any())
                    {
                        foreach (var song in trackList)
                        {
                            song.SongLiked = _searchHelper.GuidBinarySearch(songIds.ReturnData, song.ArtistTrackUploadId) != -1 ? true : false;
                        }
                    }
                }

                return new ServiceResponse<TrackListResponseDto>(HttpStatusCode.OK)
                {
                    ReturnData = new TrackListResponseDto
                    {
                        TrackList = trackList,
                        CanLoadMore = trackList.Count == 10
                    }
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(GetArtistTracksAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<TrackListResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured retrieving artist uploads, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> LikeArtistTrackAsync(Guid trackId, string artistMemberId, Guid userId, string memberId)
        {
            try
            {
                var accountResult = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!accountResult.IsSuccessStatusCode || accountResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured liking artist track, please try again..."
                    };
                }

                var likeResult = await _trackRepository.LikeArtistTrackAsync(userId, artistMemberId, trackId);

                if (!likeResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured liking artist track, please try again..."
                    };
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(LikeArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured liking artist track, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> UnikeArtistTrackAsync(Guid trackId, string artistMemberId, Guid userId, string memberId)
        {
            try
            {
                var accountResult = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!accountResult.IsSuccessStatusCode || accountResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured liking artist track, please try again..."
                    };
                }

                var unlikeResult = await _trackRepository.UnlikeArtistTrackAsync(userId, artistMemberId, trackId);

                if (!unlikeResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured liking artist track, please try again..."
                    };
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(LikeArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured unliking artist track, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> UpdateTrackSupportingDetailsAsync(Guid userId, string memberId, Guid trackId, TrackUpdateDto trackData)
        {
            try
            {
                var accountResult = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!accountResult.IsSuccessStatusCode || accountResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                var track = await _trackRepository.GetArtistTrackAsync(memberId, trackId);

                if (track is null || track.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound);
                }

                // Check if new image was added
                FileContent? newTrackImage = null;
                ServiceResponse<string>? uploadResponse = null;
                ServiceResponse? deleteResponse = null;
                string newTrackImageUrl = null;
                // If it was then upload to azure and get details
                if (trackData.TrackImage != null)
                {
                    var imageId = Guid.NewGuid();
                    var imageFilePath = $"[{userId}]/uploads/{trackId}/image/{imageId}{trackData.TrackImageExt}";

                    uploadResponse = await _blobRepository.UploadBlobAsync(imageFilePath, trackData.TrackImage);

                    if (uploadResponse != null && 
                        (!uploadResponse.IsSuccessStatusCode || 
                        uploadResponse.ReturnData is null || 
                        string.IsNullOrWhiteSpace(uploadResponse.ReturnData)))
                    {
                        return new ServiceResponse(HttpStatusCode.InternalServerError)
                        {
                            ServiceResponseMessage = "An error occured updating your track, please try again..."
                        };
                    }

                    if (track.ReturnData?.TrackImage != null)
                    {
                        deleteResponse = await _blobRepository.DeleteBlobAsync(track.ReturnData.TrackImage.FileLocation);
                    }

                    newTrackImageUrl = uploadResponse.ReturnData;
                    newTrackImage = new FileContent
                    {
                        FileContentId = imageId,
                        FileLocation = imageFilePath,
                        FileExtensionType = trackData.TrackImageExt != null ? trackData.TrackImageExt : ".jpg",
                    };
                }

                if (deleteResponse != null && !deleteResponse.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured updating your track, please try again..."
                    };
                }

                var updateResult = await _trackRepository.UpdateArtistTrackUploadAsync(accountResult.ReturnData, trackId, trackData.TrackName, trackData.TrackDescription, trackData.Genres, trackData.TrackImageExt, newTrackImage, newTrackImageUrl);

                if (updateResult is null || updateResult.StatusCode != HttpStatusCode.OK) 
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(UpdateTrackSupportingDetailsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured updating your track, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> UploadTrackSupportingDetailsAsync(Guid userId, string memberId, Guid trackId, TrackUploadDto trackUploadDto)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError);
                }

                
                var track = new ArtistTrackUpload
                {
                    AppUserId = userId,
                    ArtistTrackUploadId = trackId,
                    ArtistMemberId = memberId,
                    Genres = trackUploadDto.Genres.Select(x => new ArtistTrackGenre
                    {
                        ArtistTrackUploadId = trackId,
                        GenreId = x.GenreId
                    }).ToList(),
                    TrackName = trackUploadDto.TrackName,
                    TrackDescription = trackUploadDto.TrackDescription,
                    TrackData = new FileContent
                    {
                        FileContentId = Guid.NewGuid(),
                        FileLocation = $"[{userId}]/"+trackUploadDto.FileLocation,
                        FileExtensionType = trackUploadDto.TrackFileExt
                    },
                    TrackUrl = trackUploadDto.TrackUrl,
                    WaveformUrl = trackUploadDto.WaveformUrl,
                    Duration = double.TryParse(trackUploadDto.Duration, out var duration) ? duration : 0,
                    UploadDate = DateTime.Now.ToLocalTime(),
                    FileSizeInBytes = int.Parse(trackUploadDto.FileSize),
                };

                // If they have uploaded a custom image against a track, add it to container and db
                if (trackUploadDto.TrackImage != null)
                {
                    var imageId = Guid.NewGuid();
                    var imageFilePath = $"[{userId}]/uploads/{trackId}/image/{imageId}{trackUploadDto.TrackImageExt}";

                    var result = await _blobRepository.UploadBlobAsync(imageFilePath, trackUploadDto.TrackImage);

                    if (!result.IsSuccessStatusCode || result.ReturnData is null)
                    {
                        return new ServiceResponse(HttpStatusCode.InternalServerError);
                    }

                    track.TrackImage = new FileContent
                    {
                        FileContentId = imageId,
                        FileLocation = imageFilePath,
                        FileExtensionType = trackUploadDto.TrackImageExt
                    };

                    track.TrackImageUrl = result.ReturnData;
                }

                var addTrackResult = await _trackRepository.AddArtistTrackUploadAsync(track);

                if (!addTrackResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(UploadTrackSupportingDetailsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured uploading your track, please try again..."
                };
            }
        }
    }
}
