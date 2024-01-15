using System;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Services
{
    public interface IAccountImageService
    {
        Task<bool> DeleteAccountImage(Guid userId, int accountImageId, string fileUrl);
    }
}
