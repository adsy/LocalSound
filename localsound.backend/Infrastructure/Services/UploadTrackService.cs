using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
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

                BlobContainerClient container = new(_blobStorageSettings.ConnectionString, "tracks");

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
                    ContainerName = container.Name,
                    ContainerUri = container.Uri,
                    TrackId = trackId,
                    UploadLocation = $"[tracks]/account/{userId}/uploads/{trackId}",
                    SasUri = sasUri,
                    SasToken = sasUri.Query.TrimStart('?'),
                    SasPermission = sasBuilder.Permissions,
                    SasExpire = sasBuilder.ExpiresOn
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
                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                };
            }
        }
    }
}
