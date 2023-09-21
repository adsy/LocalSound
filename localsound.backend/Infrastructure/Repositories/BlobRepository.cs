using Azure.Storage.Blobs;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private BlobServiceClient _blobServiceClient;
        private readonly BlobStorageSettingsAdaptor _blobSettings;
        private readonly ILogger<BlobRepository> _logger;

        public BlobRepository(BlobStorageSettingsAdaptor blobSettings, ILogger<BlobRepository> logger)
        {
            _blobSettings = blobSettings;
            _logger = logger;
            _blobServiceClient = new BlobServiceClient(_blobSettings.ConnectionString);
        }

        public async Task<ServiceResponse<string>> UploadBlobAsync(string fileLocation, IFormFile file)
        {
            try
            {
                var containerName = fileLocation.Substring(0, fileLocation.IndexOf(']') + 1);
                var blobPath = fileLocation.Substring(containerName.Length);

                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName.Replace("[", string.Empty).Replace("]", string.Empty));

                await blobContainerClient.CreateIfNotExistsAsync();

                var blobClient = blobContainerClient.GetBlobClient(fileLocation);

                // Delete the file if it already exists
                await blobClient.DeleteIfExistsAsync();

                using Stream stream = file.OpenReadStream();

                var response = await blobClient.UploadAsync(stream);

                if (response.GetRawResponse().IsError)
                {
                    var message = $"{nameof(BlobRepository)} - {nameof(UploadBlobAsync)} - Error occured uploading store banner image to location:{fileLocation}";
                    _logger.LogError(message);
                    return new ServiceResponse<string>(HttpStatusCode.InternalServerError);
                }

                string uri = blobContainerClient.Uri.ToString();
                var fullUri = $"{uri}/{fileLocation}";

                return new ServiceResponse<string>(HttpStatusCode.OK)
                {
                    ReturnData = new BlobUriBuilder(new Uri(fullUri)).ToUri().ToString()
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(BlobRepository)} - {nameof(UploadBlobAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return new ServiceResponse<string>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<string>> UploadBlobAsync(string fileLocation, MemoryStream stream)
        {
            try
            {
                var containerName = fileLocation.Substring(0, fileLocation.IndexOf(']') + 1);
                var blobPath = fileLocation.Substring(containerName.Length);

                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName.Replace("[", string.Empty).Replace("]", string.Empty));

                await blobContainerClient.CreateIfNotExistsAsync();

                var blobClient = blobContainerClient.GetBlobClient(fileLocation);

                // Delete the file if it already exists
                await blobClient.DeleteIfExistsAsync();
                stream.Position = 0;
                var response = await blobClient.UploadAsync(stream);

                if (response.GetRawResponse().IsError)
                {
                    var message = $"{nameof(BlobRepository)} - {nameof(UploadBlobAsync)} - Error occured uploading store banner image to location:{fileLocation}";
                    _logger.LogError(message);
                    return new ServiceResponse<string>(HttpStatusCode.InternalServerError);
                }

                string uri = blobContainerClient.Uri.ToString();
                var fullUri = $"{uri}/{fileLocation}";

                return new ServiceResponse<string>(HttpStatusCode.OK)
                {
                    ReturnData = new BlobUriBuilder(new Uri(fullUri)).ToUri().ToString()
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(BlobRepository)} - {nameof(UploadBlobAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return new ServiceResponse<string>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> DeleteBlobAsync(string fileLocation)
        {
            try
            {
                var containerName = fileLocation.Substring(0, fileLocation.IndexOf(']') + 1);
                var blobPath = fileLocation.Substring(containerName.Length);

                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName.Replace("[", string.Empty).Replace("]", string.Empty));

                await blobContainerClient.CreateIfNotExistsAsync();

                var blobClient = blobContainerClient.GetBlobClient(fileLocation);

                // Delete the file if it already exists
                var result = await blobClient.DeleteIfExistsAsync();

                if (result.Value)
                {
                    return new ServiceResponse(HttpStatusCode.OK);
                }

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                var message = $"{nameof(BlobRepository)} - {nameof(DeleteBlobAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<Stream>> DownloadChunkBlobAsync(string fileLocation)
        {
            try
            {
                var containerName = fileLocation.Substring(0, fileLocation.IndexOf(']') + 1);
                var blobPath = fileLocation.Substring(containerName.Length);

                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName.Replace("[", string.Empty).Replace("]", string.Empty));

                var blobClient = blobContainerClient.GetBlobClient(fileLocation);

                var data = await blobClient.OpenReadAsync();

                if (data == null)
                {
                    return new ServiceResponse<Stream>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<Stream>(HttpStatusCode.OK)
                {
                    ReturnData = data
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(BlobRepository)} - {nameof(DownloadChunkBlobAsync)} - {e.Message}";
                _logger.LogError(e, message);
                return new ServiceResponse<Stream>(HttpStatusCode.InternalServerError);
            }
        }
    }
}
