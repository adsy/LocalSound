using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using Microsoft.AspNetCore.Http;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IAccountImageService
    {
        Task<ServiceResponse<string>> UploadAccountImage(AccountImageTypeEnum imageType, Guid appUserId, IFormFile photo);
    }
}
