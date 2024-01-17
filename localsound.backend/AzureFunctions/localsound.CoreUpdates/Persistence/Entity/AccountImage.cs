using LocalSound.Shared.Package.ServiceBus.Dto.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.CoreUpdates.Persistence.Entity
{

    public class AccountImage
    {
        public int AccountImageId { get; set; }
        [ForeignKey("AccountImageType")]
        public AccountImageTypeEnum AccountImageTypeId { get; set; }
        [ForeignKey("AppUser")]
        public Guid AppUserId { get; set; }
        [ForeignKey("FileContent")]
        public Guid FileContentId { get; set; }
        public string AccountImageUrl { get; set; }
        public bool ToBeDeleted { get; set; }

        public virtual FileContent FileContent { get; set; }
    }
}
