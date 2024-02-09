using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class AccountImageRepository : IAccountImageRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<AccountImageRepository> _logger;

        public AccountImageRepository(LocalSoundDbContext dbContext, ILogger<AccountImageRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<AccountImage>> MarkAccountImageToBeDeleted(AccountImageTypeEnum imageType, Guid appUserId)
        {
            try
            {
                var accountImage = await _dbContext.AccountImage
                    .Include(x => x.FileContent)
                    .FirstOrDefaultAsync(x => x.AccountImageTypeId == imageType && x.AppUserId == appUserId && !x.ToBeDeleted);

                if (accountImage is null)
                {
                    return new ServiceResponse<AccountImage>(HttpStatusCode.NotFound);
                }

                accountImage.ToBeDeleted = true;

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<AccountImage>(HttpStatusCode.OK)
                {
                    ReturnData = accountImage
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountImageRepository)} - {nameof(MarkAccountImageToBeDeleted)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<AccountImage>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<AccountImage>> UploadAccountImageAsync(AccountImageTypeEnum imageType, Guid appUserId, string fileLocation, string fileExt)
        {
            try
            {
                var accountImage = await _dbContext.AccountImage
                    .Include(x => x.FileContent)
                    .FirstOrDefaultAsync(x => x.AccountImageTypeId == imageType && x.AppUserId == appUserId);


                var fileContentId = Guid.NewGuid();

                // create new file content
                var fileContent = new FileContent
                {
                    FileContentId = fileContentId,
                    FileExtensionType = fileExt,
                    FileLocation = fileLocation + $"/{fileContentId}"
                };
                accountImage = new AccountImage
                {
                    FileContent = fileContent,
                    AppUserId = appUserId,
                    AccountImageTypeId = imageType
                };

                await _dbContext.AccountImage.AddAsync(accountImage);

                return new ServiceResponse<AccountImage>(HttpStatusCode.OK)
                {
                    ReturnData = accountImage
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountImageRepository)} - {nameof(UploadAccountImageAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<AccountImage>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while saving your profile image, please try again..."
                };
            }
        }
    }
}
