using System;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Repository
{
    public interface IDbOperationRepository
    {
        Task<bool> DeleteAccountImageAsync(Guid userId, int accountImageId);
    }
}
