using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Entity;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace localsound.backend.Persistence.DbContext
{
    public class Seed
    {
        public static async Task SeedData(LocalSoundDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(CustomerType.NonArtist.ToString()));
                await roleManager.CreateAsync(new IdentityRole<Guid>(CustomerType.Artist.ToString()));
            }
        }
    }
}
