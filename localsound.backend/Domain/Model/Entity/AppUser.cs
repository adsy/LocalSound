using localsound.backend.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace localsound.backend.Domain.Model.Entity
{
    public class AppUser : IdentityUser
    {
        public CustomerType CustomerType { get; set; }
    }
}
