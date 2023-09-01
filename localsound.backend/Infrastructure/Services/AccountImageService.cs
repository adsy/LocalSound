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
        private readonly ILogger<AccountImageService> _logger;

        public AccountImageService(IAccountImageRepository accountImageRepository, ILogger<AccountImageService> logger, IDbTransactionRepository dbTransactionRepository)
        {
            _accountImageRepository = accountImageRepository;
            _logger = logger;
            _dbTransactionRepository = dbTransactionRepository;
        }

        public async Task<ServiceResponse<string>> UploadAccountImage(AccountImageTypeEnum imageType, Guid appUserId, IFormFile photo)
        {
            try
            {
                await _dbTransactionRepository.BeginTransactionAsync();

                var fileLocation = $"[images]/account/{appUserId}/imageType/{(int)imageType}";

                // create database entries
                var accountImageResult = await _accountImageRepository.UploadAccountImageAsync(imageType, appUserId, fileLocation);

                if (!accountImageResult.IsSuccessStatusCode) 
                {
                    return new ServiceResponse<string>(accountImageResult.StatusCode, accountImageResult.ServiceResponseMessage);
                }

                // upload to azure


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
