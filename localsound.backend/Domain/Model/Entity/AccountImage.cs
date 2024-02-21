using localsound.backend.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class AccountImage
    {
        public int AccountImageId { get; set; }
        [ForeignKey(nameof(AccountImageType))]
        public AccountImageTypeEnum AccountImageTypeId { get; set; }
        [ForeignKey(nameof(AppUser))]
        public Guid AppUserId { get; set; }
        public string AccountImageUrl { get; set; }
        public bool ToBeDeleted { get; set; }

        public virtual AccountImageType AccountImageType { get; set; }
        public virtual Account AppUser { get; set; }
        public virtual AccountImageFileContent FileContent { get; set; }
    }
}
