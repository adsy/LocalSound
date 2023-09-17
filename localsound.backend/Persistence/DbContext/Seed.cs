using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace localsound.backend.Persistence.DbContext
{
    public class Seed
    {
        public static async Task SeedData(LocalSoundDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(CustomerTypeEnum.NonArtist.ToString()));
                await roleManager.CreateAsync(new IdentityRole<Guid>(CustomerTypeEnum.Artist.ToString()));
            }

            if (!(await context.AccountImageType.AnyAsync())){
                await context.AccountImageType.AddAsync(new AccountImageType
                {
                    AccountImageTypeId = AccountImageTypeEnum.ProfileImage,
                    AccountImageTypeName = "ProfileImage"
                });
                await context.AccountImageType.AddAsync(new AccountImageType
                {
                    AccountImageTypeId = AccountImageTypeEnum.CoverImage,
                    AccountImageTypeName = "CoverImage"
                });

                await context.SaveChangesAsync();
            }

            if (!(await context.EventType.AnyAsync()))
            {
                await context.EventType.AddRangeAsync(new List<EventType>
                {
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Weddings"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "House party"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Religious events"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Club residency"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Festivals"
                    }
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
