using localsound.backend.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace localsound.backend.Domain.Model.Entity
{
    public class AppUser : IdentityUser<Guid>
    {
        public CustomerTypeEnum CustomerType { get; set; }
        public string MemberId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual NonArtist NonArtist { get; set; }

        public virtual ICollection<AccountImage> Images { get; set; }
        public virtual ICollection<ArtistFollower> Following { get; set; }
    }
}
