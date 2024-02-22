using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using LocalSound.Shared.Package.ServiceBus.Dto;
using LocalSound.Shared.Package.ServiceBus.Dto.Enum;
using LocalSound.Shared.Package.ServiceBus.Dto.QueueMessage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using AccountImageTypeEnum = localsound.backend.Domain.Enum.AccountImageTypeEnum;

namespace localsound.backend.Infrastructure.Services
{
    public class AccountImageService : IAccountImageService
    {
        private readonly IDbTransactionRepository _dbTransactionRepository;
        private readonly IAccountImageRepository _accountImageRepository;
        private readonly IBlobRepository _blobRepository;
        private readonly IServiceBusRepository _serviceBusRepository;
        private readonly ILogger<AccountImageService> _logger;

        public AccountImageService(IAccountImageRepository accountImageRepository, ILogger<AccountImageService> logger, IDbTransactionRepository dbTransactionRepository, IBlobRepository blobRepository, IServiceBusRepository serviceBusRepository)
        {
            _accountImageRepository = accountImageRepository;
            _logger = logger;
            _dbTransactionRepository = dbTransactionRepository;
            _blobRepository = blobRepository;
            _serviceBusRepository = serviceBusRepository;
        }

        public async Task<ServiceResponse> DeleteAccountImageIfExists(AccountImageTypeEnum imageType, Guid appUserId)
        {
            try
            {
                await _dbTransactionRepository.BeginTransactionAsync();

                var imageResult = await _accountImageRepository.MarkAccountImageToBeDeleted(imageType, appUserId);

                if (!imageResult.IsSuccessStatusCode || imageResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound);
                }

                // Add the delete entity message to be handled by azure function
                var deleteMessage = new ServiceBusMessageDto<DeleteAccountImageDto>
                {
                    Command = DeleteEntityTypeEnum.DeleteAccountImage,
                    Data = new DeleteAccountImageDto
                    {
                        UserId = appUserId,
                        AccountImageId = imageResult.ReturnData.AccountImageId
                    }
                };

                var queueResult = await _serviceBusRepository.SendDeleteQueueEntry(deleteMessage);

                if (!queueResult.IsSuccessStatusCode)
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

        public async Task<ServiceResponse<string>> UploadAccountImage(AccountImageTypeEnum imageType, Guid appUserId, IFormFile photo, string fileExt)
        {
            try
            {
                await _dbTransactionRepository.BeginTransactionAsync();

                var fileLocation = $"[{appUserId}]/photos/imageType/{(int)imageType}";

                // create database entries
                var accountImageResult = await _accountImageRepository.UploadAccountImageAsync(imageType, appUserId, fileLocation, fileExt);

                if (!accountImageResult.IsSuccessStatusCode || accountImageResult.ReturnData is null) 
                {
                    return new ServiceResponse<string>(accountImageResult.StatusCode, accountImageResult.ServiceResponseMessage);
                }

                // upload to azure
                var blobUploadResult = await _blobRepository.UploadBlobAsync(accountImageResult.ReturnData.FileContent.FileLocation+$"{fileExt}", photo);

                if (!blobUploadResult.IsSuccessStatusCode || blobUploadResult.ReturnData is null)
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
