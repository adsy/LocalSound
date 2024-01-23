using localsound.CoreUpdates.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Services
{
    public class PackageService : IPackageService
    {
        private readonly IDbOperationRepository _dbOperationRepository;
        private readonly IBlobRepository _blobRepository;

        public PackageService(IDbOperationRepository dbOperationRepository, IBlobRepository blobRepository)
        {
            _dbOperationRepository = dbOperationRepository;
            _blobRepository = blobRepository;
        }

        public async Task<bool> DeletePackagePhotos(Guid userId, Guid packageId, List<string> photoLocations)
        {
            foreach(var photo in photoLocations)
            {
                var result = await _blobRepository.DeleteEntityFromStorage(photo);

                if (!result)
                    return false;
            }

            var dbDeleteResult = await _dbOperationRepository.DeletePackagePhotosAsync(userId, packageId);

            if (!dbDeleteResult)
                return dbDeleteResult;

            return true;
        }
    }
}
