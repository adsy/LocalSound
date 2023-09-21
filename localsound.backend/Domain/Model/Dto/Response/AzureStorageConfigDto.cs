namespace localsound.backend.Domain.Model.Dto.Response
{
    public class AzureStorageConfig
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}
