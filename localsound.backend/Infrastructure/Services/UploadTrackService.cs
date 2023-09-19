using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class UploadTrackService : IUploadTrackService
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUploadTrackRepository _uploadTrackRepository;
        private readonly IDbTransactionRepository _dbTransactionRepository;
        private readonly ILogger<UploadTrackService> _logger;

        public UploadTrackService(IBlobRepository blobRepository, ILogger<UploadTrackService> logger, IAccountRepository accountRepository, IUploadTrackRepository uploadTrackRepository, IDbTransactionRepository dbTransactionRepository)
        {
            _blobRepository = blobRepository;
            _logger = logger;
            _accountRepository = accountRepository;
            _uploadTrackRepository = uploadTrackRepository;
            _dbTransactionRepository = dbTransactionRepository;
        }

        public async Task<ServiceResponse> UploadFileChunk(Guid userId, string memberId, Guid partialTrackId, IFormFile file, int chunkId)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, "There was an error while updating your details, please try again.");
                }

                await _dbTransactionRepository.BeginTransactionAsync();

                var fileLocation = $"[tracks]/account/{userId}/chunks/partialTrackId-{partialTrackId}";

                var addToDbResult = await _uploadTrackRepository.UploadTrackChunkAsync(fileLocation, partialTrackId, userId, chunkId);

                if (!addToDbResult.IsSuccessStatusCode || addToDbResult.ReturnData == null)
                {
                    // Log error here?
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                var uploadResult = await _blobRepository.UploadBlobAsync(addToDbResult.ReturnData.FileContent.FileLocation, file);

                if (!uploadResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(uploadResult.StatusCode);
                }

                await _dbTransactionRepository.CommitTransactionAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(UploadTrackService)} - {nameof(UploadFileChunk)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while uploading your track, please try again.");
            }
        }
    }
}
