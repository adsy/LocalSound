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

        public async Task<ServiceResponse<int>> AddArtistTrackUploadAsync(ArtistTrack track)
        {
            try
            {
                var trackEntity = await _dbContext.ArtistTrack.AddAsync(track);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<int>(HttpStatusCode.OK)
                {
                    ReturnData = trackEntity.Entity.ArtistTrackId
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(AddArtistTrackUploadAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<int>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> MarkTrackForDeletion(string memberId, int trackId)
        {
            try
            {
                var track = await _dbContext.ArtistTrack.Include(x => x.SongLikes).FirstOrDefaultAsync(x => x.ArtistMemberId == memberId && x.ArtistTrackId == trackId);

                if (track == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                _dbContext.SongLike.RemoveRange(track.SongLikes);

                track.ToBeDeleted = true;

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(MarkTrackForDeletion)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<ArtistTrack>> GetArtistTrackAsync(string memberId, int trackId)
        {
            try
            {
                var track = await _dbContext.ArtistTrack
                    .Include(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.TrackData)
                    .Include(x => x.TrackImage)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.SongLikes)
                    .FirstOrDefaultAsync(x => x.ArtistMemberId == memberId && x.ArtistTrackId == trackId);

                if (track is null)
                {
                    return new ServiceResponse<ArtistTrack>(HttpStatusCode.NotFound);
                }

                return new ServiceResponse<ArtistTrack>(HttpStatusCode.OK)
                {
                    ReturnData = track
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<ArtistTrack>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetArtistTracksAsync(string memberId, int? lastTrackId)
        {
            try
            {
                var tracksQuery = _dbContext.ArtistTrack.AsQueryable();

                if (lastTrackId.HasValue) {
                    tracksQuery = tracksQuery.Where(x => x.ArtistMemberId == memberId && x.ArtistTrackId < lastTrackId && !x.ToBeDeleted);
                }
                else
                {
                    tracksQuery = tracksQuery.Where(x => x.ArtistMemberId == memberId && !x.ToBeDeleted);
                }

                var tracks = await tracksQuery
                    .Include(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.TrackData)
                    .Include(x => x.TrackImage)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .OrderByDescending(x => x.ArtistTrackId)
                    .Select(x => new ArtistTrackUploadDto
                    {
                        ArtistTrackId = x.ArtistTrackId,
                        TrackName = x.TrackName,
                        TrackDescription = x.TrackDescription,
                        TrackImageUrl = x.TrackImage != null && x.TrackImage.FirstOrDefault(x => !x.ToBeDeleted) != null ? x.TrackImage.FirstOrDefault(x => !x.ToBeDeleted).TrackImageUrl : x.Artist.Images != null && x.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage) != null ? x.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage).AccountImageUrl : "",
                        ArtistProfile = x.Artist.ProfileUrl,
                        ArtistName = x.Artist.Name,
                        ArtistMemberId = x.Artist.MemberId,
                        TrackUrl = x.TrackUrl,
                        Duration = x.Duration,
                        UploadDate = x.UploadDate,
                        LikeCount = x.LikeCount,
                        Genres = x.Genres.Select(genre => new GenreDto
                        {
                            GenreId = genre.GenreId,
                            GenreName = genre.Genre.GenreName
                        }).ToList()
                    })
                    .Take(10).ToListAsync();

                return new ServiceResponse<List<ArtistTrackUploadDto>>(HttpStatusCode.OK)
                {
                    ReturnData = tracks
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetArtistTracksAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistTrackUploadDto>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetLikedSongsAsync(string memberId, int? lastTrackId)
        {
            try
            {
                var tracksQuery = _dbContext.SongLike.AsQueryable();

                if (lastTrackId.HasValue)
                {
                    tracksQuery = tracksQuery.Where(x => x.MemberId == memberId && x.SongLikeId < lastTrackId && !x.ArtistTrack.ToBeDeleted);
                }
                else
                {
                    tracksQuery = tracksQuery.Where(x => x.MemberId == memberId && !x.ArtistTrack.ToBeDeleted);
                }

                var tracks = await tracksQuery
                    .Include(x => x.ArtistTrack)
                    .ThenInclude(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.ArtistTrack)
                    .ThenInclude(x => x.TrackData)
                    .Include(x => x.ArtistTrack)
                    .ThenInclude(x => x.TrackImage)
                    .Include(x => x.ArtistTrack)
                    .ThenInclude(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .OrderByDescending(x => x.SongLikeId)
                    .Select(x => new ArtistTrackUploadDto
                    {
                        ArtistTrackId = x.ArtistTrack.ArtistTrackId,
                        TrackName = x.ArtistTrack.TrackName,
                        TrackDescription = x.ArtistTrack.TrackDescription,
                        TrackImageUrl = x.ArtistTrack.TrackImage != null && x.ArtistTrack.TrackImage.FirstOrDefault(x => !x.ToBeDeleted) != null ? x.ArtistTrack.TrackImage.FirstOrDefault(x => !x.ToBeDeleted).TrackImageUrl : x.ArtistTrack.Artist.Images != null && x.ArtistTrack.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage) != null ? x.ArtistTrack.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage).AccountImageUrl : "",
                        ArtistProfile = x.ArtistTrack.Artist.ProfileUrl,
                        ArtistName = x.ArtistTrack.Artist.Name,
                        ArtistMemberId = x.ArtistTrack.Artist.MemberId,
                        TrackUrl = x.ArtistTrack.TrackUrl,
                        Duration = x.ArtistTrack.Duration,
                        UploadDate = x.ArtistTrack.UploadDate,
                        LikeCount = x.ArtistTrack.LikeCount,
                        SongLikeId = x.SongLikeId,
                        Genres = x.ArtistTrack.Genres.Select(genre => new GenreDto
                        {
                            GenreId = genre.GenreId,
                            GenreName = genre.Genre.GenreName
                        }).ToList()
                    })
                    .Take(10)
                    .ToListAsync();

                return new ServiceResponse<List<ArtistTrackUploadDto>>(HttpStatusCode.OK)
                {
                    ReturnData = tracks
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetLikedSongsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistTrackUploadDto>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<int>>> GetLikedSongsIdsAsync(string memberId)
        {
            try
            {
                var songLikes = await _dbContext.SongLike.Where(x => x.MemberId == memberId).Select(x => x.ArtistTrackId).ToListAsync();

                return new ServiceResponse<List<int>>(HttpStatusCode.OK)
                {
                    ReturnData = songLikes
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(GetLikedSongsIdsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<int>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> LikeArtistTrackAsync(string memberId, string artistMemberId, int trackId)
        {
            try
            {
                await _dbContext.SongLike.AddAsync(new SongLike
                {
                    ArtistTrackId = trackId,
                    MemberId = memberId,
                });

                await _dbContext.SaveChangesAsync();

                var track = await _dbContext.ArtistTrack.FirstOrDefaultAsync(x => x.ArtistTrackId == trackId && x.ArtistMemberId == artistMemberId);
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

        public async Task<ServiceResponse> UnlikeArtistTrackAsync(string memberId, int songLikeId)
        {
            try
            {
                var songLike = await _dbContext.SongLike.Include(x => x.ArtistTrack).FirstOrDefaultAsync(x => x.MemberId == memberId && x.SongLikeId == songLikeId);

                if (songLike is null)
                    return new ServiceResponse(HttpStatusCode.InternalServerError);

                _dbContext.SongLike.Remove(songLike);

                await _dbContext.SaveChangesAsync();

                if (songLike.ArtistTrack != null)
                {
                    bool saveFailed = false;
                    do
                    {
                        songLike.ArtistTrack.LikeCount--;

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

        public async Task<ServiceResponse> UpdateArtistTrackUploadAsync(Account account, int trackId, string trackName, string trackDescription, List<GenreDto> genres, string? trackImageExt, ArtistTrackImageFileContent? newTrackImage, string newTrackImageUrl)
        {
            try
            {
                var track = await _dbContext.ArtistTrack
                    .Include(x => x.Genres)
                    .Include(x => x.TrackImage)
                    .FirstOrDefaultAsync(x => x.AppUserId == account.AppUserId && x.ArtistTrackId == trackId);

                if (track is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound);
                }

                track.TrackName = trackName;
                track.TrackDescription = trackDescription;
                track.Genres = genres.Select(x => new ArtistTrackGenre
                {
                    GenreId = x.GenreId
                }).ToList();

                //if (newTrackImage != null)
                //{
                //    await _dbContext.ArtistTrackImageFileContent.AddAsync(newTrackImage);

                //    if (track.TrackImage != null)
                //    {
                //        _dbContext.ArtistTrackImageFileContent.Remove(track.TrackImage);
                //    }

                //    track.TrackImage = newTrackImage;
                //    track.TrackImageUrl = newTrackImageUrl;
                //}

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
