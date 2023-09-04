using localsound.backend.Domain.Model;
using Microsoft.AspNetCore.Http;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IBlobRepository
    {
        Task<ServiceResponse<string>> UploadBlobAsync(string fileLocation, IFormFile file);
    }
}
