using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace localsound.backend.Persistence.DbContext
{
    public class LocalSoundDbContext : IdentityDbContext<AppUser,
        IdentityRole, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public LocalSoundDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<NonArtist> NonArtist { get; set; }
        public DbSet<Artist> Artist { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<NonArtist>().HasKey(x => x.AppUserId);
            builder.Entity<Artist>().HasKey(x => x.AppUserId);
        }

        public async Task<ServiceResponse> HandleSavingDB()
        {
            var result = await SaveChangesAsync() > 0;

            if (result)
                return new ServiceResponse(HttpStatusCode.OK);

            return new ServiceResponse(HttpStatusCode.InternalServerError);
        }
    }
}
