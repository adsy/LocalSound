using localsound.backend.Domain.Model;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface IBlobRepository
    {
        Task<ServiceResponse> DeleteBlobAsync(string fileLocation);
        Task<ServiceResponse<string>> UploadBlobAsync(string fileLocation, IFormFile file);
        Task<ServiceResponse<string>> UploadBlobAsync(string fileLocation, MemoryStream stream);
        Task<ServiceResponse<Stream>> DownloadChunkBlobAsync(string fileLocation);
    }
}
