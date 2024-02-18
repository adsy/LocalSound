using localsound.CoreUpdates.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace localsound.CoreUpdates.Persistence
{
    public class LocalSoundDbContext : DbContext
    {
        public LocalSoundDbContext(DbContextOptions<LocalSoundDbContext> options) : base(options)
        {

        }

        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistPackage> ArtistPackage { get; set; }
        public DbSet<ArtistPackagePhoto> ArtistPackagePhoto { get; set; }
        public DbSet<AccountImage> AccountImage { get; set; }
        public DbSet<FileContent> FileContent { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ArtistBooking>().HasKey(x => x.BookingId).IsClustered(false);
            builder.Entity<ArtistBooking>().HasIndex(x => x.ArtistId).IsClustered(true);
            builder.Entity<ArtistBooking>().Property(x => x.BookingCompleted).HasDefaultValue(false);

            builder.Entity<ArtistPackage>().HasKey(x => x.ArtistPackageId).IsClustered(false);
            builder.Entity<ArtistPackage>().HasIndex(x => x.AppUserId).IsClustered(true);

            builder.Entity<ArtistPackagePhoto>().HasKey(x => x.ArtistPackagePhotoId).IsClustered(false);
            builder.Entity<ArtistPackagePhoto>().HasIndex(x => x.ArtistPackageId).IsClustered(true);


            builder.Entity<AccountImage>(x =>
            {
                x.HasKey(x => x.AccountImageId);
            });

            builder.Entity<FileContent>(x =>
            {
                x.HasKey(x => x.FileContentId);
                x.HasOne(x => x.Image).WithOne(x => x.FileContent).OnDelete(DeleteBehavior.Cascade);
                x.HasOne(x => x.ArtistPackagePhoto).WithOne(x => x.FileContent).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
