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
        public DbSet<ArtistTrackUpload> ArtistTrackUpload { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<FileContent> FileContent { get; set; }
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

            builder.Entity<NonArtist>().HasKey(x => x.AppUserId);
            builder.Entity<NonArtist>().HasIndex(x => x.ProfileUrl).IsUnique();

            builder.Entity<Artist>().HasKey(x => x.AppUserId);
            builder.Entity<Artist>().HasMany(x => x.Genres);
            builder.Entity<Artist>().HasIndex(x => x.ProfileUrl).IsUnique();

            builder.Entity<AppUserToken>().Property(o => o.ExpirationDate).HasDefaultValueSql("DateAdd(week,1,getDate())");

            builder.Entity<Genre>().HasKey(x => x.GenreId);


            builder.Entity<ArtistGenre>().HasKey(x => new {x.AppUserId, x.GenreId});
            builder.Entity<ArtistGenre>().HasOne(x => x.Genre);
            builder.Entity<ArtistGenre>().HasOne(x => x.Artist);

            builder.Entity<AccountImageType>().HasKey(x => x.AccountImageTypeId);

            builder.Entity<AccountImage>(x =>
            {
                x.HasKey(x => x.AccountImageId);
            });

            builder.Entity<FileContent>().HasKey(x => x.FileContentId);
            builder.Entity<ArtistTrackUpload>().HasKey(x => x.ArtistTrackUploadId);
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
