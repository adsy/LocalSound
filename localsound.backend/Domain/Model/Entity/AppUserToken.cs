using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class AppUserToken : IdentityUserToken<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ExpirationDate { get; set; }
    }
}
