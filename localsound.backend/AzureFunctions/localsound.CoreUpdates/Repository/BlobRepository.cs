using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace localsound.CoreUpdates.Repository
{
    public class BlobRepository : IBlobRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<BlobRepository> _logger;

        public BlobRepository(BlobServiceClient blobServiceClient, ILogger<BlobRepository> logger)
        {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public async Task<bool> DeleteEntityFromStorage(string fileLocation)
        {
            try
            {
                var containerName = fileLocation.Substring(0, fileLocation.IndexOf(']') + 1);
                var blobPath = fileLocation.Substring(containerName.Length);

                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName.Replace("[", string.Empty).Replace("]", string.Empty));

                await blobContainerClient.CreateIfNotExistsAsync();

                var blobClient = blobContainerClient.GetBlobClient(blobPath);

                // If it doesnt exist then we dont need to delete it
                if (!(await blobClient.ExistsAsync()))
                {
                    return true;
                }

                // Delete the file if it already exists
                var result = await blobClient.DeleteIfExistsAsync();

                if (result.Value)
                {
                    return true;
                }

                return false;
            }
            catch(Exception e)
            {
                var message = $"{nameof(BlobRepository)} - {nameof(DeleteEntityFromStorage)} - {e.Message}";
                _logger.LogError(e, message);
                return false;
            }
        }
    }
}
