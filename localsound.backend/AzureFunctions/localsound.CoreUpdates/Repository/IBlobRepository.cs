using System.Threading.Tasks;

namespace localsound.CoreUpdates.Repository
{
    public interface IBlobRepository
    {
        Task<bool> DeleteEntityFromStorage(string fileLocation);
    }
}
