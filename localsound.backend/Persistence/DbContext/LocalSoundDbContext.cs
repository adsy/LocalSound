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

        public DbSet<AccountGenre> AccountGenre { get; set; }
        public DbSet<AccountImage> AccountImage { get; set; }
        public DbSet<AccountImageFileContent> AccountImageFileContent { get; set; }
        public DbSet<AccountImageType> AccountImageType { get; set; }
        public DbSet<AccountMessages> AccountMessages { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserToken> AppUserToken { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<ArtistBooking> ArtistBooking { get; set; }
        public DbSet<ArtistEquipment> ArtistEquipment { get; set; }
        public DbSet<ArtistEventType> ArtistEventType { get; set; }
        public DbSet<ArtistFollower> ArtistFollower { get; set; }
        public DbSet<ArtistPackage> ArtistPackage { get; set; }
        public DbSet<ArtistPackageEquipment> ArtistPackageEquipment { get; set; }
        public DbSet<ArtistPackageImage> ArtistPackageImage { get; set; }
        public DbSet<ArtistPackageImageFileContent> ArtistPackageImageFileContent { get; set; }
        public DbSet<ArtistTrackGenre> ArtistTrackGenre { get; set; }
        public DbSet<ArtistTrack> ArtistTrack { get; set; }
        public DbSet<ArtistTrackAudioFileContent> ArtistTrackAudioFileContent { get; set; }
        public DbSet<ArtistTrackImageFileContent> ArtistTrackImageFileContent { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<SongLike> SongLike { get; set; }
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
            builder.Entity<Account>(x =>
            {
                x.Property(x => x.MemberId).HasDefaultValueSql("NEXT VALUE FOR MemberId");
                x.HasIndex(x => x.MemberId).IsUnique(true).IsClustered(false);
                x.HasMany(x => x.Following);
                x.HasOne(x => x.AccountMessages).WithOne(x => x.Account);
                x.HasMany(x => x.Following).WithOne(x => x.Follower).OnDelete(DeleteBehavior.NoAction);
                x.HasMany(x => x.Followers).WithOne(x => x.Artist).OnDelete(DeleteBehavior.NoAction);
                x.HasKey(x => x.AppUserId);
                x.HasMany(x => x.Genres);
                x.HasIndex(x => x.ProfileUrl).IsUnique();
                x.HasMany(x => x.Packages).WithOne(x => x.Artist).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AppUserToken>().Property(o => o.ExpirationDate).HasDefaultValueSql("DateAdd(week,1,getDate())");

            builder.Entity<AccountMessages>(x =>
            {
                x.HasKey(x => x.AppUserId);
                x.Property(x => x.OnboardingMessageClosed).HasDefaultValue(false);
            });


            builder.Entity<ArtistTrackGenre>(x =>
            {
                x.HasKey(x => new { x.ArtistTrackId, x.GenreId });
                x.HasOne(x => x.Genre);
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

            builder.Entity<Genre>().HasKey(x => x.GenreId);
            builder.Entity<EventType>().HasKey(x => x.EventTypeId);

            builder.Entity<ArtistBooking>(x =>
            {
                x.HasKey(x => x.BookingId).IsClustered(false);
                x.Property(x => x.BookingId).ValueGeneratedOnAdd();
                x.HasIndex(x => x.ArtistId).IsClustered(true);
                x.Property(x => x.BookingCompleted).HasDefaultValue(false);
                x.HasOne(x => x.Artist).WithMany(x => x.Bookings).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(x => x.Booker).WithMany(x => x.PartiesBooked).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(x => x.Package).WithMany(x => x.RelatedBookings).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(x => x.EventType).WithMany(x => x.RelatedBookings).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<ArtistEventType>(x =>
            {
                x.HasKey(x => new { x.AppUserId, x.EventTypeId });
                x.HasOne(x => x.EventType);
                x.HasOne(x => x.Artist);
            });

            builder.Entity<AccountGenre>(x =>
            {
                x.HasKey(x => new { x.AppUserId, x.GenreId });
                x.HasOne(x => x.Genre);
                x.HasOne(x => x.Artist);
            });

            builder.Entity<ArtistEquipment>(x =>
            {
                x.HasKey(x => new { x.AppUserId, x.EquipmentId }).IsClustered(false);
                x.HasOne(x => x.Artist);
                x.HasIndex(x => x.AppUserId).IsClustered();
            });

            builder.Entity<ArtistPackage>(x =>
            {
                x.HasKey(x => x.ArtistPackageId).IsClustered(false);
                x.HasIndex(x => x.AppUserId).IsClustered(true);
                x.HasMany(x => x.Equipment).WithOne(x => x.ArtistPackage).OnDelete(DeleteBehavior.Cascade);
                x.HasMany(x => x.PackagePhotos).WithOne(x => x.ArtistPackage).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ArtistPackageEquipment>(x =>
            {
                x.HasKey(x => new { x.ArtistPackageId, x.ArtistPackageEquipmentId }).IsClustered(false);
                x.HasOne(x => x.ArtistPackage);
                x.HasIndex(x => x.ArtistPackageId).IsClustered(true);
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

            builder.Entity<AccountImageType>().HasKey(x => x.AccountImageTypeId);

            builder.Entity<AccountImage>(x =>
            {
                x.HasKey(x => x.AccountImageId);
                x.HasOne(x => x.FileContent).WithOne(x => x.Image).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AccountImageFileContent>(x =>
            {
                x.HasKey(x => x.FileContentId);
                x.HasOne(x => x.Image).WithOne(x => x.FileContent);
            });

            builder.Entity<ArtistFollower>(x =>
            {
                x.HasKey(key => new { key.ArtistId, key.FollowerId });
                x.HasOne(x => x.Artist).WithMany(x => x.Followers).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(x => x.Follower).WithMany(x => x.Following).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Notification>(x =>
            {
                x.HasKey(x => x.NotificationId).IsClustered(false);
                x.Property(x => x.NotificationId).ValueGeneratedOnAdd();
                x.HasIndex(x => x.NotificationReceiverId).IsUnique(false).IsClustered(true);
                x.HasOne(x => x.NotificationReceiver).WithMany(x => x.ReceivedNotifications).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(x => x.NotificationCreator).WithMany(x => x.SentNotifications).OnDelete(DeleteBehavior.NoAction);
            });

            builder.HasSequence("SongLikeId", x => x.StartsAt(1).IncrementsBy(1));
            builder.Entity<SongLike>(x =>
            {
                x.HasKey(x => x.SongLikeId).IsClustered(false);
                x.HasAlternateKey(x => new { x.ArtistTrackId, x.MemberId }).IsClustered(false);
                x.HasOne(x => x.Account).WithMany(x => x.LikedSongs).HasPrincipalKey(x => x.MemberId).HasForeignKey(x => x.MemberId);
                x.HasOne(x => x.ArtistTrack).WithMany(x => x.SongLikes).OnDelete(DeleteBehavior.NoAction);
                x.HasIndex(x => new { x.MemberId , x.SongLikeId }).IsUnique(false).IsClustered(true);
                x.HasIndex(x => x.ArtistTrackId).IsUnique(false).IsClustered(false);
                x.Property(x => x.SongLikeId).HasDefaultValueSql("NEXT VALUE FOR SongLikeId");
            });

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
