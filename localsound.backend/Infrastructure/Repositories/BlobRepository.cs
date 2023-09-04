using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

namespace localsound.backend.Infrastructure.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private BlobServiceClient _blobServiceClient;
        private readonly BlobStorageSettingsAdaptor _blobSettings;
        private readonly ILogger<BlobRepository> _logger;

        public BlobRepository(IOptions<BlobStorageSettingsAdaptor> blobSettings, ILogger<BlobRepository> logger)
        {
            _blobSettings = blobSettings.Value;
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
            catch(Exception ex)
            {
                return new ServiceResponse<string>(System.Net.HttpStatusCode.OK);
            }
        }
    }
}
