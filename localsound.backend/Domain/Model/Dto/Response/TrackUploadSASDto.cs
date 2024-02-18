namespace localsound.backend.Domain.Model.Dto.Response
{
    public class TrackUploadSASDto
    {
        public string AccountName { get; set; }
        public string AccountUrl { get; set; }
        public Uri ContainerUri { get; set; }
        public string ContainerName { get; set; }
        public string UploadLocation { get; set; }
        public int trackId { get; set; }
        public Uri SasUri { get; set; }
        public string SasToken { get; set; }
        public string SasPermission { get; set; }
        public DateTimeOffset SasExpire { get; set; }
    }
}
