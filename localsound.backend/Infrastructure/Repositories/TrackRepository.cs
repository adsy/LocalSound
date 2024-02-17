using Azure;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        public readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<TrackRepository> _logger;

        public TrackRepository(LocalSoundDbContext dbContext, ILogger<TrackRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> AddArtistTrackUploadAsync(ArtistTrackUpload track)
        {
            try
            {
                await _dbContext.ArtistTrackUpload.AddAsync(track);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(AddArtistTrackUploadAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> DeleteTrackAsync(ArtistTrackUpload track)
        {
            try
            {
                _dbContext.SongLike.RemoveRange(track.SongLikes);

                _dbContext.FileContent.Remove(track.TrackData);

                if (track.TrackImage != null)
                {
                    _dbContext.FileContent.Remove(track.TrackImage);
                }

                _dbContext.ArtistTrackUpload.Remove(track);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(DeleteTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(string memberId, Guid trackId)
        {
            try
            {
                var artist = await _dbContext.Account.FirstOrDefaultAsync(x => x.MemberId == memberId && x.CustomerType == CustomerTypeEnum.Artist);

                if (artist is null)
                {
                    return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.NotFound);
                }

                var track = await _dbContext.ArtistTrackUpload
                    .Include(x => x.TrackData)
                    .Include(x => x.TrackImage)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.SongLikes)
                    .FirstOrDefaultAsync(x => x.AppUserId == artist.AppUserId && x.ArtistTrackUploadId == trackId);

                if (track is null)
                {
                    return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.OK)
                {
                    ReturnData = track
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistTrackUpload>>> GetArtistTracksAsync(string memberId, DateTime? lastUploadDate)
        {
            try
            {
                var artist = await _dbContext.Account.FirstOrDefaultAsync(x => x.MemberId == memberId && x.CustomerType == CustomerTypeEnum.Artist);

                if (artist is null)
                {
                    return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.NotFound);
                }

                var date = lastUploadDate.HasValue ? lastUploadDate.Value : DateTime.Now;

                var tracks = await _dbContext.ArtistTrackUpload
                    .Include(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.TrackData)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Where(x => x.AppUserId == artist.AppUserId && x.UploadDate <  date)
                    .OrderByDescending(x => x.UploadDate)
                    .Take(10)
                    .ToListAsync();

                return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.OK)
                {
                    ReturnData = tracks
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetArtistTracksAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistTrackUpload>>> GetLikedSongsAsync(string memberId, DateTime? lastUploadDate)
        {
            try
            {
                var date = lastUploadDate.HasValue ? lastUploadDate.Value : DateTime.Now;

                var tracks = await _dbContext.SongLike
                    .Include(x => x.ArtistTrackUpload)
                    .ThenInclude(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.ArtistTrackUpload)
                    .ThenInclude(x => x.TrackData)
                    .Include(x => x.ArtistTrackUpload)
                    .ThenInclude(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Where(x => x.MemberId == memberId && x.ArtistTrackUpload.UploadDate < date)
                    .OrderByDescending(x => x.SongLikeId)
                    .Select(x => x.ArtistTrackUpload)
                    .Take(10)
                    .ToListAsync();

                return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.OK)
                {
                    ReturnData = tracks
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetLikedSongsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistTrackUpload>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<Guid>>> GetLikedSongsIdsAsync(string memberId)
        {
            try
            {
                var songLikes = await _dbContext.SongLike.Where(x => x.MemberId == memberId).Select(x => x.ArtistTrackId).ToListAsync();

                return new ServiceResponse<List<Guid>>(HttpStatusCode.OK)
                {
                    ReturnData = songLikes
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetLikedSongsIdsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<Guid>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> LikeArtistTrackAsync(string memberId, string artistMemberId, Guid trackId)
        {
            try
            {
                await _dbContext.SongLike.AddAsync(new SongLike
                {
                    ArtistTrackId = trackId,
                    MemberId = memberId,
                });

                await _dbContext.SaveChangesAsync();

                var track = await _dbContext.ArtistTrackUpload.FirstOrDefaultAsync(x => x.ArtistTrackUploadId == trackId && x.ArtistMemberId == artistMemberId);
                if (track != null)
                {
                    bool saveFailed = false;
                    do
                    {
                        track.LikeCount++;

                        try
                        {
                            await _dbContext.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException e)
                        {
                            saveFailed = true;
                            e.Entries.Single().Reload();
                        }
                    } while (saveFailed);
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(LikeArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> UnlikeArtistTrackAsync(string memberId, string artistMemberId, Guid trackId)
        {
            try
            {
                var songLike = await _dbContext.SongLike.FirstOrDefaultAsync(x => x.ArtistTrackId == trackId && x.MemberId == memberId);

                if (songLike is null)
                    return new ServiceResponse(HttpStatusCode.InternalServerError);

                _dbContext.SongLike.Remove(songLike);

                await _dbContext.SaveChangesAsync();

                var trackLikeCount = await _dbContext.ArtistTrackUpload.FirstOrDefaultAsync(x => x.ArtistTrackUploadId == trackId && x.ArtistMemberId == artistMemberId);
                if (trackLikeCount != null)
                {
                    bool saveFailed = false;
                    do
                    {
                        trackLikeCount.LikeCount--;

                        try
                        {
                            await _dbContext.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException e)
                        {
                            saveFailed = true;
                            e.Entries.Single().Reload();
                        }
                    } while (saveFailed);
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(UnlikeArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> UpdateArtistTrackUploadAsync(Account account, Guid trackId, string trackName, string trackDescription, List<GenreDto> genres, string? trackImageExt, FileContent? newTrackImage, string newTrackImageUrl)
        {
            try
            {
                var track = await _dbContext.ArtistTrackUpload
                    .Include(x => x.Genres)
                    .Include(x => x.TrackImage)
                    .FirstOrDefaultAsync(x => x.AppUserId == account.AppUserId && x.ArtistTrackUploadId == trackId);

                if (track is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound);
                }

                track.TrackName = trackName;
                track.TrackDescription = trackDescription;
                track.Genres = genres.Select(x => new ArtistTrackGenre
                {
                    ArtistTrackUploadId = trackId,
                    GenreId = x.GenreId
                }).ToList();

                if (newTrackImage != null)
                {
                    await _dbContext.FileContent.AddAsync(newTrackImage);

                    if (track.TrackImage != null)
                    {
                        _dbContext.FileContent.Remove(track.TrackImage);
                    }

                    track.TrackImage = newTrackImage;
                    track.TrackImageUrl = newTrackImageUrl;
                }

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(UpdateArtistTrackUploadAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
