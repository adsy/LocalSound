using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class AccountImageService : IAccountImageService
    {
        private readonly IDbTransactionRepository _dbTransactionRepository;
        private readonly IAccountImageRepository _accountImageRepository;
        private readonly IBlobRepository _blobRepository;
        private readonly ILogger<AccountImageService> _logger;

        public AccountImageService(IAccountImageRepository accountImageRepository, ILogger<AccountImageService> logger, IDbTransactionRepository dbTransactionRepository, IBlobRepository blobRepository)
        {
            _accountImageRepository = accountImageRepository;
            _logger = logger;
            _dbTransactionRepository = dbTransactionRepository;
            _blobRepository = blobRepository;
        }

        public async Task<ServiceResponse> DeleteAccountImageIfExists(AccountImageTypeEnum imageType, Guid appUserId)
        {
            try
            {
                await _dbTransactionRepository.BeginTransactionAsync();

                var fileLocation = $"[{appUserId}]/photos/imageType/{(int)imageType}";

                // create database entries
                var deleteImageResult = await _accountImageRepository.DeleteAccountImageAsync(imageType, appUserId);

                if (!deleteImageResult.IsSuccessStatusCode || deleteImageResult.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound);
                }
                // upload to azure
                var blobDeleteResult = await _blobRepository.DeleteBlobAsync(fileLocation + $"/{deleteImageResult.ReturnData}");

                if (!blobDeleteResult.IsSuccessStatusCode)
                {
                    // If it fails here theres something wrong with Azure
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                await _dbTransactionRepository.CommitTransactionAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountImageService)} - {nameof(DeleteAccountImageIfExists)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<string>> UploadAccountImage(AccountImageTypeEnum imageType, Guid appUserId, IFormFile photo)
        {
            try
            {
                await _dbTransactionRepository.BeginTransactionAsync();

                var fileLocation = $"[{appUserId}]/photos/imageType/{(int)imageType}";

                // create database entries
                var accountImageResult = await _accountImageRepository.UploadAccountImageAsync(imageType, appUserId, fileLocation);

                if (!accountImageResult.IsSuccessStatusCode || accountImageResult.ReturnData == null) 
                {
                    return new ServiceResponse<string>(accountImageResult.StatusCode, accountImageResult.ServiceResponseMessage);
                }

                var fileExt = photo.FileName.Split(".")[^1];

                // upload to azure
                var blobUploadResult = await _blobRepository.UploadBlobAsync(accountImageResult.ReturnData.FileContent.FileLocation+$".{fileExt}", photo);

                if (!blobUploadResult.IsSuccessStatusCode || blobUploadResult.ReturnData == null)
                {
                    // If it fails here theres something wrong with Azure
                    return new ServiceResponse<string>(HttpStatusCode.InternalServerError);
                }

                accountImageResult.ReturnData.AccountImageUrl = blobUploadResult.ReturnData;

                await _dbTransactionRepository.CommitTransactionAsync();

                return new ServiceResponse<string>(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountImageService)} - {nameof(UploadAccountImage)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<string>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while saving your profile image, please try again..."
                };
            }
        }
    }
}
