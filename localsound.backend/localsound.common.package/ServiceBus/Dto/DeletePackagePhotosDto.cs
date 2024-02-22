namespace LocalSound.Shared.Package.ServiceBus.Dto
{
    public class DeletePackagePhotosDto
    {
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
    }
}
