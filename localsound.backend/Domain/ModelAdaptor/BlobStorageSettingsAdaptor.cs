namespace localsound.backend.Domain.ModelAdaptor
{
    public class BlobStorageSettingsAdaptor
    {
        public const string BlobSettings = "AzureBlobStorage";

        public string ConnectionString { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}
