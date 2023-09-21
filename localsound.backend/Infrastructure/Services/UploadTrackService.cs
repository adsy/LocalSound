using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class UploadTrackService : IUploadTrackService
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUploadTrackRepository _uploadTrackRepository;
        private readonly BlobStorageSettingsAdaptor _blobStorageSettings;
        private readonly ILogger<UploadTrackService> _logger;

        public UploadTrackService(IBlobRepository blobRepository, IAccountRepository accountRepository, IUploadTrackRepository uploadTrackRepository, BlobStorageSettingsAdaptor blobStorageSettings, ILogger<UploadTrackService> logger)
        {
            _blobRepository = blobRepository;
            _accountRepository = accountRepository;
            _uploadTrackRepository = uploadTrackRepository;
            _blobStorageSettings = blobStorageSettings;
            _logger = logger;
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

                BlobServiceClient service = new (_blobStorageSettings.ConnectionString);

                if (!service.CanGenerateAccountSasUri)
                    return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError, "The container can't generate SAS URI");

                var sasUri = service.GenerateAccountSasUri(AccountSasPermissions.All, DateTimeOffset.UtcNow.AddMinutes(_blobStorageSettings.TokenExpirationMinutes), AccountSasResourceTypes.Object);

                var trackId = Guid.NewGuid();

                var result = new TrackUploadSASDto
                {
                    AccountName = service.AccountName,
                    AccountUrl = $"{service.Uri.Scheme}://{service.Uri.Host}",
                    ContainerName = "tracks",
                    TrackId = trackId,
                    UploadLocation = $"account/{userId}/uploads/{trackId}",
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
                var message = $"{nameof(UploadTrackService)} - {nameof(GenerateTrackUploadSASDto)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<TrackUploadSASDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured, please try again..."
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
                var imageFilePath = $"account/{userId}/uploads/{trackId}/image/{imageId}{trackUploadDto.TrackFileExt}";

                var result = await _blobRepository.UploadBlobAsync(imageFilePath, trackUploadDto.TrackImage);

                if (!result.IsSuccessStatusCode || result.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                var track = new ArtistTrackUpload
                {
                    AppUserId = userId,
                    ArtistTrackUploadId = trackId,
                    GenreId = trackUploadDto.GenreId,
                    TrackName = trackUploadDto.TrackName,
                    TrackDescription = trackUploadDto.TrackDescription,
                    TrackData = new FileContent
                    {
                        FileContentId = Guid.NewGuid(),
                        FileLocation = trackUploadDto.FileLocation,
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

                var addTrackResult = await _uploadTrackRepository.AddArtistTrackUploadAsync(track);

                if (!addTrackResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackService)} - {nameof(UploadTrackSupportingDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured uploading your track, please try again..."
                };
            }
        }
    }
}
