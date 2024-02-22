using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using localsound.backend.Domain.Enum;
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

        public async Task<ServiceResponse> DeleteArtistTrackAsync(Guid userId, string memberId, int trackId)
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

                var dbDeleteResult = await _trackRepository.MarkTrackForDeletion(memberId, trackId);

                if (dbDeleteResult is null || !dbDeleteResult.IsSuccessStatusCode)
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your track, please try again..."
                    };

                //TODO: Push service bus message

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

        public async Task<ServiceResponse<ArtistTrackUploadDto>> GetArtistTrackAsync(string memberId, int trackId)
        {
            try
            {
                var track = await _trackRepository.GetArtistTrackAsync(memberId, trackId);

                if (!track.IsSuccessStatusCode || track.ReturnData is null)
                {
                    return new ServiceResponse<ArtistTrackUploadDto>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured retrieving track details, please try again..."
                    };
                }

                var trackImageUrl = "";
                var trackImage = track.ReturnData.TrackImage?.FirstOrDefault(x => !x.ToBeDeleted);

                if (trackImage == null)
                {
                    trackImageUrl = track.ReturnData.Artist.Images?.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage && !x.ToBeDeleted).AccountImageUrl;
                }
                else
                {
                    trackImageUrl = trackImage.TrackImageUrl;
                }

                var returnData = new ArtistTrackUploadDto
                {
                    ArtistTrackId = track.ReturnData.ArtistTrackId,
                    TrackName = track.ReturnData.TrackName,
                    TrackDescription = track.ReturnData.TrackDescription,
                    TrackImageUrl = trackImageUrl,
                    ArtistProfile = track.ReturnData.Artist.ProfileUrl,
                    ArtistName = track.ReturnData.Artist.Name,
                    ArtistMemberId = track.ReturnData.Artist.MemberId,
                    TrackUrl = track.ReturnData.TrackUrl,
                    Duration = track.ReturnData.Duration,
                    UploadDate = track.ReturnData.UploadDate,
                    LikeCount = track.ReturnData.LikeCount,
                    Genres = track.ReturnData.Genres.Select(genre => new GenreDto
                    {
                        GenreId = genre.GenreId,
                        GenreName = genre.Genre.GenreName
                    }).ToList()
                };

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

        public async Task<ServiceResponse<TrackListResponseDto>> GetTracksByPlaylistTypeAsync(Guid? userId, string memberId, int? lastTrackId, PlaylistTypeEnum playlistType)
        {
            try
            {
                ServiceResponse<List<ArtistTrackUploadDto>> tracksResult = null;

                switch (playlistType)
                {
                    case PlaylistTypeEnum.Uploads:
                        {
                            tracksResult = await _trackRepository.GetArtistTracksAsync(memberId, lastTrackId);
                            break;
                        }
                    case PlaylistTypeEnum.Favourites:
                        {
                            tracksResult = await _trackRepository.GetLikedSongsAsync(memberId, lastTrackId);
                            break;
                        }
                    default:
                        {
                            // Invalid playlist type
                            tracksResult = new ServiceResponse<List<ArtistTrackUploadDto>>(HttpStatusCode.InternalServerError);
                            break;
                        }
                }


                if (!tracksResult.IsSuccessStatusCode || tracksResult.ReturnData is null)
                {
                    return new ServiceResponse<TrackListResponseDto>(tracksResult.StatusCode);
                }

                if (userId is not null)
                {
                    var loggedInUser = await _accountRepository.GetAccountFromDbAsync(userId);

                    if (loggedInUser.ReturnData is not null)
                    {
                        var songIds = (await _trackRepository.GetLikedSongsIdsAsync(loggedInUser.ReturnData.MemberId));

                        if (songIds.IsSuccessStatusCode && songIds.ReturnData is not null && songIds.ReturnData.Any())
                        {
                            songIds.ReturnData = songIds.ReturnData.OrderBy(x => x).ToList();
                            foreach (var song in tracksResult.ReturnData)
                            {
                                song.SongLiked = _searchHelper.IntBinarySearch(songIds.ReturnData, song.ArtistTrackId) != -1 ? true : false;
                            }
                        }
                    }
                }

                return new ServiceResponse<TrackListResponseDto>(HttpStatusCode.OK)
                {
                    ReturnData = new TrackListResponseDto
                    {
                        TrackList = tracksResult.ReturnData,
                        CanLoadMore = tracksResult.ReturnData.Count == 10
                    }
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(GetTracksByPlaylistTypeAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<TrackListResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured retrieving artist uploads, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> LikeArtistTrackAsync(int trackId, string artistMemberId, Guid userId, string memberId)
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

                var likeResult = await _trackRepository.LikeArtistTrackAsync(memberId, artistMemberId, trackId);

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

        public async Task<ServiceResponse> UnlikeArtistTrackAsync(Guid userId, string memberId, int songLikeId)
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

                var unlikeResult = await _trackRepository.UnlikeArtistTrackAsync(memberId, songLikeId);

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

        public async Task<ServiceResponse> UpdateTrackSupportingDetailsAsync(Guid userId, string memberId, int trackId, TrackUpdateDto trackData)
        {
            try
            {
                var accountResult = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!accountResult.IsSuccessStatusCode || accountResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                var trackResult = await _trackRepository.GetArtistTrackAsync(memberId, trackId);

                if (trackResult is null || trackResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound);
                }

                ServiceResponse? updateResult = null;
                if (trackData.TrackImage != null)
                {
                    var imageId = Guid.NewGuid();
                    var imageFilePath = $"{trackResult.ReturnData.TrackData.FileLocation}/image/{imageId}{trackData.TrackImageExt}";

                    var uploadResponse = await _blobRepository.UploadBlobAsync(imageFilePath, trackData.TrackImage);

                    if (!uploadResponse.IsSuccessStatusCode || string.IsNullOrWhiteSpace(uploadResponse.ReturnData))
                    {
                        return new ServiceResponse(HttpStatusCode.InternalServerError)
                        {
                            ServiceResponseMessage = "An error occured updating your track, please try again..."
                        };
                    }

                    var newTrackImage = new ArtistTrackImage
                    {
                        ArtistTrack = trackResult.ReturnData,
                        TrackImageUrl = uploadResponse.ReturnData,
                        ArtistTrackImageFileContent = new ArtistTrackImageFileContent
                        {
                            FileContentId = imageId,
                            FileLocation = imageFilePath,
                            FileExtensionType = trackData.TrackImageExt != null ? trackData.TrackImageExt : ".jpg",
                        }
                    };

                    updateResult = await _trackRepository.UpdateArtistTrackUploadAsync(trackResult.ReturnData, trackData.TrackName, trackData.TrackDescription, trackData.Genres, newTrackImage);

                    // TODO: Refactor this to push to service bus queue
                    //var oldImage = trackResult.ReturnData.TrackImage?.FirstOrDefault(x => x.ToBeDeleted);
                    //if (oldImage != null)
                    //{
                    //    var deleteResponse = await _blobRepository.DeleteBlobAsync(oldImage.ArtistTrackImageFileContent.FileLocation);

                    //    if (deleteResponse != null && !deleteResponse.IsSuccessStatusCode)
                    //    {
                    //        return new ServiceResponse(HttpStatusCode.InternalServerError)
                    //        {
                    //            ServiceResponseMessage = "An error occured updating your track, please try again..."
                    //        };
                    //    }
                    //}
                }
                else
                {
                    updateResult = await _trackRepository.UpdateArtistTrackUploadAsync(trackResult.ReturnData, trackData.TrackName, trackData.TrackDescription, trackData.Genres, null);
                }

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

        public async Task<ServiceResponse<int>> UploadTrackSupportingDetailsAsync(Guid userId, string memberId, TrackUploadDto trackUploadDto)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse<int>(HttpStatusCode.InternalServerError);
                }
                
                var track = new ArtistTrack
                {
                    AppUserId = userId,
                    ArtistMemberId = memberId,
                    Genres = trackUploadDto.Genres.Select(x => new ArtistTrackGenre
                    {
                        GenreId = x.GenreId
                    }).ToList(),
                    TrackName = trackUploadDto.TrackName,
                    TrackDescription = trackUploadDto.TrackDescription,
                    TrackData = new ArtistTrackAudioFileContent
                    {
                        FileContentId = Guid.NewGuid(),
                        FileLocation = $"[{userId}]/"+trackUploadDto.FileLocation,
                        FileExtensionType = trackUploadDto.TrackFileExt
                    },
                    TrackUrl = trackUploadDto.TrackUrl,
                    Duration = double.TryParse(trackUploadDto.Duration, out var duration) ? duration : 0,
                    UploadDate = DateTime.Now.ToLocalTime(),
                    FileSizeInBytes = int.Parse(trackUploadDto.FileSize),
                };

                // If they have uploaded a custom image against a track, add it to container and db
                if (trackUploadDto.TrackImage != null)
                {
                    var imageId = Guid.NewGuid();
                    var imageFilePath = $"{track.TrackData.FileLocation}/image/{imageId}{trackUploadDto.TrackImageExt}";

                    var result = await _blobRepository.UploadBlobAsync(imageFilePath, trackUploadDto.TrackImage);

                    if (!result.IsSuccessStatusCode || result.ReturnData is null)
                    {
                        return new ServiceResponse<int>(HttpStatusCode.InternalServerError);
                    }

                    track.TrackImage = new List<ArtistTrackImage>
                    {
                        new ArtistTrackImage
                        {
                            TrackImageUrl = result.ReturnData,
                            ArtistTrackImageFileContent = new ArtistTrackImageFileContent
                            {
                                FileContentId = imageId,
                                FileLocation = imageFilePath,
                                FileExtensionType = trackUploadDto.TrackImageExt
                            }
                        }
                    };
                }

                var addTrackResult = await _trackRepository.AddArtistTrackUploadAsync(track);

                if (!addTrackResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse<int>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<int>(HttpStatusCode.OK)
                {
                    ReturnData = addTrackResult.ReturnData
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(UploadTrackSupportingDetailsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<int>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured uploading your track, please try again..."
                };
            }
        }
    }
}
