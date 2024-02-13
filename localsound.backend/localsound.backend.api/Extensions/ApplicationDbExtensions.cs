using localsound.backend.Domain.Model.Entity;
using localsound.backend.Persistence.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace localsound.backend.api.Extensions
{
    public static class ApplicationDbExtensions
    {
        public static async Task AddDbSeed(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<LocalSoundDbContext>();
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await context.Database.MigrateAsync();
            await Seed.SeedData(context, userManager, roleManager);
        }
    }
}
