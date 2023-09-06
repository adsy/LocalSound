namespace localsound.backend.Domain.Model.Entity
{
    public class FileContent
    {
        public Guid FileContentId { get; set; }
        public string FileLocation { get; set; }
        public string FileExtensionType { get; set; }

        public virtual AccountImage Image { get; set; }
    }
}
