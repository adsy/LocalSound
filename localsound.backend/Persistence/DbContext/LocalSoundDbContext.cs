using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Net;

namespace localsound.backend.Persistence.DbContext
{
    public class LocalSoundDbContext : IdentityDbContext<AppUser,
        IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, AppUserToken>
    {

        private IDbContextTransaction _currentTransaction;

        public LocalSoundDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<AccountImage> AccountImage { get; set; }
        public DbSet<AccountImageType> AccountImageType { get; set; }
        public DbSet<AccountOnboarding> AccountOnboarding { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserToken> AppUserToken { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistEquipment> ArtistEquipment { get; set; }
        public DbSet<ArtistEventType> ArtistEventType { get; set; }
        public DbSet<ArtistFollower> ArtistFollower { get; set; }
        public DbSet<ArtistGenre> ArtistGenre { get; set; }
        public DbSet<ArtistPackage> ArtistPackage { get; set; }
        public DbSet<ArtistPackageEquipment> ArtistPackageEquipment { get; set; }
        public DbSet<ArtistPackagePhoto> ArtistPackagePhoto { get; set; }
        public DbSet<ArtistTrackGenre> ArtistTrackGenre { get; set; }
        public DbSet<ArtistTrackUpload> ArtistTrackUpload { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<FileContent> FileContent { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Account> NonArtist { get; set; }
        public DbSet<Notification> Notification { get; set; }
        

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence("MemberId", x => x.StartsAt(100000).IncrementsBy(1));
            builder.Entity<Account>().Property(x => x.MemberId).HasDefaultValueSql("NEXT VALUE FOR MemberId");
            builder.Entity<Account>().HasMany(x => x.Following);
            builder.Entity<Account>().HasOne(x => x.AccountOnboarding).WithOne(x => x.Account);

            builder.Entity<Account>().HasMany(x => x.Following).WithOne(x => x.Follower).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Account>().HasMany(x => x.Followers).WithOne(x => x.Artist).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<AppUserToken>().Property(o => o.ExpirationDate).HasDefaultValueSql("DateAdd(week,1,getDate())");

            builder.Entity<AccountOnboarding>().HasKey(x => x.AppUserId);

            builder.Entity<Account>().HasKey(x => x.AppUserId);
            builder.Entity<Account>().HasMany(x => x.Genres);
            builder.Entity<Account>().HasIndex(x => x.ProfileUrl).IsUnique();

            builder.Entity<Account>().HasMany(x => x.Packages).WithOne(x => x.Artist).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ArtistTrackGenre>().HasKey(x => new { x.ArtistTrackUploadId, x.GenreId });
            builder.Entity<ArtistTrackGenre>().HasOne(x => x.Genre);
            builder.Entity<ArtistTrackGenre>().HasOne(x => x.ArtistTrackUpload);

            builder.Entity<ArtistTrackUpload>().HasKey(x => x.ArtistTrackUploadId).IsClustered(false);
            builder.Entity<ArtistTrackUpload>().HasIndex(x => x.AppUserId).IsClustered(true);
            builder.Entity<ArtistTrackUpload>().HasMany(x => x.Genres);
            builder.Entity<ArtistTrackUpload>().HasOne(x => x.TrackData).WithOne(x => x.ArtistTrackUpload).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Genre>().HasKey(x => x.GenreId);
            builder.Entity<EventType>().HasKey(x => x.EventTypeId);

            builder.Entity<ArtistBooking>().HasKey(x => x.BookingId).IsClustered(false);
            builder.Entity<ArtistBooking>().HasOne(x => x.Artist).WithMany(x => x.Bookings).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ArtistBooking>().HasOne(x => x.Booker).WithMany(x => x.PartiesBooked).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ArtistBooking>().HasOne(x => x.Package).WithMany(x => x.RelatedBookings).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ArtistBooking>().HasOne(x => x.EventType).WithMany(x => x.RelatedBookings).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ArtistBooking>().HasIndex(x => x.ArtistId).IsClustered(true);
            builder.Entity<ArtistBooking>().Property(x => x.BookingCompleted).HasDefaultValue(false);

            builder.Entity<ArtistEventType>().HasKey(x => new { x.AppUserId, x.EventTypeId });
            builder.Entity<ArtistEventType>().HasOne(x => x.EventType);
            builder.Entity<ArtistEventType>().HasOne(x => x.Artist);

            builder.Entity<ArtistGenre>().HasKey(x => new { x.AppUserId, x.GenreId });
            builder.Entity<ArtistGenre>().HasOne(x => x.Genre);
            builder.Entity<ArtistGenre>().HasOne(x => x.Artist);

            builder.Entity<ArtistEquipment>().HasKey(x => new { x.AppUserId, x.EquipmentId }).IsClustered(false);
            builder.Entity<ArtistEquipment>().HasOne(x => x.Artist);
            builder.Entity<ArtistEquipment>().HasIndex(x => x.AppUserId).IsClustered();

            builder.Entity<ArtistPackage>().HasKey(x => x.ArtistPackageId).IsClustered(false);
            builder.Entity<ArtistPackage>().HasIndex(x => x.AppUserId).IsClustered(true);
            builder.Entity<ArtistPackage>().HasMany(x => x.Equipment).WithOne(x => x.ArtistPackage).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ArtistPackage>().HasMany(x => x.PackagePhotos).WithOne(x => x.ArtistPackage).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ArtistPackageEquipment>().HasKey(x => new { x.ArtistPackageId, x.ArtistPackageEquipmentId }).IsClustered(false);
            builder.Entity<ArtistPackageEquipment>().HasOne(x => x.ArtistPackage);
            builder.Entity<ArtistPackageEquipment>().HasIndex(x => x.ArtistPackageId).IsClustered(true);

            builder.Entity<ArtistPackagePhoto>().HasKey(x => x.ArtistPackagePhotoId).IsClustered(false);
            builder.Entity<ArtistPackagePhoto>().HasIndex(x => x.ArtistPackageId).IsClustered(true);

            builder.Entity<Account>().HasKey(x => x.AppUserId);
            builder.Entity<Account>().HasIndex(x => x.ProfileUrl).IsUnique();

            builder.Entity<AccountImageType>().HasKey(x => x.AccountImageTypeId);

            builder.Entity<AccountImage>(x =>
            {
                x.HasKey(x => x.AccountImageId);
            });

            builder.Entity<FileContent>(x =>
            {
                x.HasKey(x => x.FileContentId);
                x.HasOne(x => x.Image).WithOne(x => x.FileContent).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ArtistFollower>(x =>
            {
                x.HasKey(key => new { key.ArtistId, key.FollowerId });
                x.HasOne(x => x.Artist).WithMany(x => x.Followers).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(x => x.Follower).WithMany(x => x.Following).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Notification>().HasKey(x => x.NotificationId).IsClustered(false);
            builder.Entity<Notification>().HasIndex(x => x.NotificationReceiverId).IsUnique(false).IsClustered(true);
            builder.Entity<Notification>().HasOne(x => x.NotificationReceiver).WithMany(x => x.ReceivedNotifications).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Notification>().HasOne(x => x.NotificationCreator).WithMany(x => x.SentNotifications).OnDelete(DeleteBehavior.NoAction);
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
