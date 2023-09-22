using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

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

        public TrackService(IBlobRepository blobRepository, IAccountRepository accountRepository, ITrackRepository trackRepository, BlobStorageSettingsAdaptor blobStorageSettings, ILogger<TrackService> logger, IMapper mapper)
        {
            _blobRepository = blobRepository;
            _accountRepository = accountRepository;
            _trackRepository = trackRepository;
            _blobStorageSettings = blobStorageSettings;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<TrackUploadSASDto>> GenerateTrackUploadSASDto(Guid userId, string memberId)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError);
                }

                BlobContainerClient container = new (_blobStorageSettings.ConnectionString, userId.ToString());

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
                var message = $"{nameof(TrackService)} - {nameof(GenerateTrackUploadSASDto)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetArtistTracks(string memberId)
        {
            try
            {
                var tracks = await _trackRepository.GetArtistTracksAsync(memberId);

                if (!tracks.IsSuccessStatusCode || tracks.ReturnData == null)

                {
                    return new ServiceResponse<List<ArtistTrackUploadDto>>(tracks.StatusCode);
                }

                return new ServiceResponse<List<ArtistTrackUploadDto>>(HttpStatusCode.OK)
                {
                    ReturnData = _mapper.Map<List<ArtistTrackUploadDto>>(tracks.ReturnData)
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(GetArtistTracks)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistTrackUploadDto>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured uploading your track, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> UploadTrackSupportingDetails(Guid userId, string memberId, Guid trackId, TrackUploadDto trackUploadDto)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError);
                }

                var imageId = Guid.NewGuid();
                var imageFilePath = $"[{userId}]/uploads/{trackId}/image/{imageId}{trackUploadDto.TrackImageExt}";

                var result = await _blobRepository.UploadBlobAsync(imageFilePath, trackUploadDto.TrackImage);

                if (!result.IsSuccessStatusCode || result.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                //var genres = JsonSerializer.Deserialize<List<GenreDto>>(trackUploadDto.Genres);

                var track = new ArtistTrackUpload
                {
                    AppUserId = userId,
                    ArtistTrackUploadId = trackId,
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
                        FileLocation = "[tracks]/"+trackUploadDto.FileLocation,
                        FileExtensionType = trackUploadDto.TrackFileExt
                    },
                    TrackImage = new FileContent
                    {
                        FileContentId = imageId,
                        FileLocation = imageFilePath,
                        FileExtensionType = trackUploadDto.TrackImageExt
                    },
                    TrackImageUrl = result.ReturnData
                };

                var addTrackResult = await _trackRepository.AddArtistTrackUploadAsync(track);

                if (!addTrackResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackService)} - {nameof(UploadTrackSupportingDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured uploading your track, please try again..."
                };
            }
        }
    }
}
