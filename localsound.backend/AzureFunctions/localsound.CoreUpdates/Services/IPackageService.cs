using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Services
{
    public interface IPackageService
    {
        Task<bool> DeletePackagePhotos(Guid userId, Guid packageId, List<string> photoLocations);
    }
}
