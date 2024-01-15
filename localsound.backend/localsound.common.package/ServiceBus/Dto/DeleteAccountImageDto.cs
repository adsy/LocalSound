namespace LocalSound.Shared.Package.ServiceBus.Dto
{
    public class DeleteAccountImageDto
    {
        public Guid UserId { get; set; }
        public int AccountImageId { get; set; }
        public string UploadLocation { get; set; }
    }
}
