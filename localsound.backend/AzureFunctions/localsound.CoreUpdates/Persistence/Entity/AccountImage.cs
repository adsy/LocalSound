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
        public string AccountImageUrl { get; set; }
        public bool ToBeDeleted { get; set; }

        public virtual AccountImageFileContent AccountImageFileContent { get; set; }
    }
}
