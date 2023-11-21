using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IAccountImageRepository
    {
        Task<ServiceResponse<string>> DeleteAccountImageAsync(AccountImageTypeEnum imageType, Guid appUserId);
        Task<ServiceResponse<AccountImage>> UploadAccountImageAsync(AccountImageTypeEnum imageType, Guid appUserId, string fileLocation, string fileExt);
    }
}
