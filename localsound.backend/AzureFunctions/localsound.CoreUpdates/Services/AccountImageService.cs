﻿using localsound.CoreUpdates.Repository;
using System;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Services
{
    public class AccountImageService : IAccountImageService
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IDbOperationRepository _dbOperationRepository;

        public AccountImageService(IBlobRepository blobRepository, IDbOperationRepository dbOperationRepository)
        {
            _blobRepository = blobRepository;
            _dbOperationRepository = dbOperationRepository;
        }

        public async Task<bool> DeleteAccountImage(Guid userId, int accountImageId, string fileUrl)
        {
            var azureDeleteOp = await _blobRepository.DeleteEntityFromStorage(fileUrl);

            if (!azureDeleteOp)
                return azureDeleteOp;

            var dbDeleteResult =  await _dbOperationRepository.DeleteAccountImageAsync(userId, accountImageId);

            if (!dbDeleteResult)
                return dbDeleteResult;

            return true; 
        }
    }
}
