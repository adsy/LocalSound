using localsound.CoreUpdates.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace localsound.CoreUpdates.Persistence
{
    public class LocalSoundDbContext : DbContext
    {
        public LocalSoundDbContext(DbContextOptions<LocalSoundDbContext> options) : base(options)
        {

        }

        public DbSet<AccountImage> AccountImage { get; set; }
        public DbSet<AccountImageFileContent> AccountImageFileContent { get; set; }
        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistPackage> ArtistPackage { get; set; }
        public DbSet<ArtistPackageImage> ArtistPackageImage { get; set; }
        public DbSet<ArtistPackageImageFileContent> ArtistPackageImageFileContent { get; set; }
        public DbSet<ArtistTrackGenre> ArtistTrackGenre { get; set; }
        public DbSet<ArtistTrack> ArtistTrack { get; set; }
        public DbSet<ArtistTrackImage> ArtistTrackImage { get; set; }
        public DbSet<ArtistTrackAudioFileContent> ArtistTrackAudioFileContent { get; set; }
        public DbSet<ArtistTrackImageFileContent> ArtistTrackImageFileContent { get; set; }
        public DbSet<SongLike> SongLike { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ArtistBooking>(x =>
            {
                x.HasKey(x => x.BookingId).IsClustered(false);
                x.Property(x => x.BookingId).ValueGeneratedOnAdd();
                x.HasIndex(x => x.ArtistId).IsClustered(true);
                x.Property(x => x.BookingCompleted).HasDefaultValue(false);
            });

            builder.Entity<ArtistPackage>(x =>
            {
                x.HasKey(x => x.ArtistPackageId).IsClustered(false);
                x.HasIndex(x => x.AppUserId).IsClustered(true);
                x.HasMany(x => x.PackagePhotos).WithOne(x => x.ArtistPackage).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ArtistPackageImage>(x =>
            {
                x.HasKey(x => x.ArtistPackageImageId).IsClustered(false);
                x.Property(x => x.ArtistPackageImageId).ValueGeneratedOnAdd();
                x.HasIndex(x => x.ArtistPackageId).IsClustered(true);
                x.HasOne(x => x.ArtistPackageImageFileContent).WithOne(x => x.ArtistPackageImage).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ArtistPackageImageFileContent>(x =>
            {
                x.HasKey(x => x.FileContentId);
                x.HasOne(x => x.ArtistPackageImage).WithOne(x => x.ArtistPackageImageFileContent);
            });

            builder.Entity<AccountImage>(x =>
            {
                x.HasKey(x => x.AccountImageId);
                x.HasOne(x => x.AccountImageFileContent).WithOne(x => x.Image).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AccountImageFileContent>(x =>
            {
                x.HasKey(x => x.FileContentId);
                x.HasOne(x => x.Image).WithOne(x => x.AccountImageFileContent);
            }); 
            
            builder.Entity<ArtistTrackGenre>(x =>
            {
                x.HasKey(x => new { x.ArtistTrackId, x.GenreId });
                x.HasOne(x => x.ArtistTrack);
            });

            builder.Entity<ArtistTrack>(x =>
            {
                x.HasKey(x => x.ArtistTrackId).IsClustered(false);
                x.Property(x => x.ArtistTrackId).ValueGeneratedOnAdd();
                x.HasIndex(x => x.ArtistMemberId).IsClustered(true);
                x.HasMany(x => x.Genres);
                x.HasOne(x => x.TrackData).WithOne(x => x.ArtistTrack).OnDelete(DeleteBehavior.Cascade);
                x.HasMany(x => x.TrackImage).WithOne(x => x.ArtistTrack).OnDelete(DeleteBehavior.Cascade);
                x.HasMany(x => x.SongLikes).WithOne(x => x.ArtistTrack).OnDelete(DeleteBehavior.Cascade);
                x.Property(x => x.LikeCount).HasDefaultValue(0).IsConcurrencyToken();
                x.Property(x => x.ToBeDeleted).HasDefaultValue(false);
            });

            builder.Entity<ArtistTrackImage>(x =>
            {
                x.HasKey(x => x.ArtistTrackImageId).IsClustered(false);
                x.Property(x => x.ArtistTrackImageId).ValueGeneratedOnAdd();
                x.HasIndex(x => x.ArtistTrackId).IsUnique(false).IsClustered(true);
                x.HasOne(x => x.ArtistTrack).WithMany(x => x.TrackImage);
                x.HasOne(x => x.ArtistTrackImageFileContent).WithOne(x => x.ArtistTrackImage).OnDelete(DeleteBehavior.Cascade);
                x.Property(x => x.ToBeDeleted).HasDefaultValue(false);
            });

            builder.Entity<ArtistTrackAudioFileContent>(x =>
            {
                x.HasKey(x => x.FileContentId);
                x.HasOne(x => x.ArtistTrack).WithOne(x => x.TrackData);
            });

            builder.Entity<ArtistTrackImageFileContent>(x =>
            {
                x.HasKey(x => x.FileContentId);
                x.HasOne(x => x.ArtistTrackImage).WithOne(x => x.ArtistTrackImageFileContent);
            });

            builder.Entity<SongLike>(x =>
            {
                x.HasKey(x => x.SongLikeId).IsClustered(false);
                x.HasAlternateKey(x => new { x.ArtistTrackId, x.MemberId }).IsClustered(false);
                x.HasOne(x => x.ArtistTrack).WithMany(x => x.SongLikes).OnDelete(DeleteBehavior.NoAction);
                x.HasIndex(x => new { x.MemberId, x.SongLikeId }).IsUnique(false).IsClustered(true);
                x.HasIndex(x => x.ArtistTrackId).IsUnique(false).IsClustered(false);
                x.Property(x => x.SongLikeId).HasDefaultValueSql("NEXT VALUE FOR SongLikeId");
            });
        }
    }
}
