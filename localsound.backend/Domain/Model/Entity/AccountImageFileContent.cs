using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class AccountImageFileContent
    {
        public Guid FileContentId { get; set; }
        [ForeignKey("Account")]
        public int AccountImageId { get; set; }
        public string FileLocation { get; set; }
        public string FileExtensionType { get; set; }

        public virtual AccountImage Image { get; set; }
    }
}
