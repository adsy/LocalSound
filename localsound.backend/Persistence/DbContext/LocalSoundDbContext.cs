using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace localsound.backend.Persistence.DbContext
{
    public class LocalSoundDbContext : IdentityDbContext<AppUser,
        IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, AppUserToken>
    {
        public LocalSoundDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserToken> AppUserToken { get; set; }
        public DbSet<NonArtist> NonArtist { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence("MemberId", x => x.StartsAt(100000).IncrementsBy(1));
            builder.Entity<AppUser>().Property(x => x.MemberId).HasDefaultValueSql("NEXT VALUE FOR MemberId");

            builder.Entity<NonArtist>().HasKey(x => x.AppUserId);
            
            builder.Entity<Artist>().HasKey(x => x.AppUserId);
            builder.Entity<Artist>().HasMany(x => x.Genres);

            builder.Entity<AppUserToken>().Property(o => o.ExpirationDate).HasDefaultValueSql("DateAdd(week,1,getDate())");

            builder.Entity<Genre>().HasKey(x => x.GenreId);


            builder.Entity<ArtistGenre>().HasKey(x => new {x.AppUserId, x.GenreId});
            builder.Entity<ArtistGenre>().HasOne(x => x.Genre);
            builder.Entity<ArtistGenre>().HasOne(x => x.Artist);
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
