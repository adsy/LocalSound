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
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserToken> AppUserToken { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<ArtistEquipment> ArtistEquipment { get; set; }
        public DbSet<ArtistEventType> ArtistEventType { get; set; }
        public DbSet<ArtistGenre> ArtistGenre { get; set; }
        public DbSet<ArtistTrackGenre> ArtistTrackGenre { get; set; }
        public DbSet<ArtistTrackUpload> ArtistTrackUpload { get; set; }
        public DbSet<EventType> EventType { get; set; }
        public DbSet<FileContent> FileContent { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<NonArtist> NonArtist { get; set; }
        

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
            builder.Entity<AppUser>().Property(x => x.MemberId).HasDefaultValueSql("NEXT VALUE FOR MemberId");

            builder.Entity<AppUserToken>().Property(o => o.ExpirationDate).HasDefaultValueSql("DateAdd(week,1,getDate())");

            builder.Entity<Artist>().HasKey(x => x.AppUserId);
            builder.Entity<Artist>().HasMany(x => x.Genres);
            builder.Entity<Artist>().HasIndex(x => x.ProfileUrl).IsUnique();

            builder.Entity<ArtistTrackGenre>().HasKey(x => new { x.ArtistTrackUploadId, x.GenreId });
            builder.Entity<ArtistTrackGenre>().HasOne(x => x.Genre);
            builder.Entity<ArtistTrackGenre>().HasOne(x => x.ArtistTrackUpload);

            builder.Entity<ArtistTrackUpload>().HasKey(x => x.ArtistTrackUploadId).IsClustered(false);
            builder.Entity<ArtistTrackUpload>().HasIndex(x => x.AppUserId).IsClustered(true);
            builder.Entity<ArtistTrackUpload>().HasMany(x => x.Genres);
            builder.Entity<ArtistTrackUpload>().HasOne(x => x.TrackData).WithOne(x => x.ArtistTrackUpload).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Genre>().HasKey(x => x.GenreId);
            builder.Entity<EventType>().HasKey(x => x.EventTypeId);

            builder.Entity<ArtistEventType>().HasKey(x => new { x.AppUserId, x.EventTypeId });
            builder.Entity<ArtistEventType>().HasOne(x => x.EventType);
            builder.Entity<ArtistEventType>().HasOne(x => x.Artist);
            builder.Entity<ArtistGenre>().HasKey(x => new { x.AppUserId, x.GenreId });
            builder.Entity<ArtistGenre>().HasOne(x => x.Genre);
            builder.Entity<ArtistGenre>().HasOne(x => x.Artist);
            builder.Entity<ArtistEquipment>().HasKey(x => new { x.AppUserId, x.EquipmentId }).IsClustered(false);
            builder.Entity<ArtistEquipment>().HasOne(x => x.Artist);
            builder.Entity<ArtistEquipment>().HasIndex(x => x.AppUserId).IsClustered();

            builder.Entity<NonArtist>().HasKey(x => x.AppUserId);
            builder.Entity<NonArtist>().HasIndex(x => x.ProfileUrl).IsUnique();

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

            //builder.Entity<ArtistFollower>(x =>
            //{
            //    x.HasKey(key => new { key.Follower, key.Followee });
            //});
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
