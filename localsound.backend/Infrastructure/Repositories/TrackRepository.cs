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

        public async Task<ServiceResponse<int>> AddArtistTrackUploadAsync(ArtistTrackUpload track)
        {
            try
            {
                var trackEntity = await _dbContext.ArtistTrackUpload.AddAsync(track);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<int>(HttpStatusCode.OK)
                {
                    ReturnData = trackEntity.Entity.ArtistTrackUploadId
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TrackRepository)} - {nameof(AddArtistTrackUploadAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<int>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> DeleteTrackAsync(ArtistTrackUpload track)
        {
            try
            {
                _dbContext.SongLike.RemoveRange(track.SongLikes);

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

        public async Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(string memberId, int trackId)
        {
            try
            {
                var track = await _dbContext.ArtistTrackUpload
                    .Include(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.TrackData)
                    .Include(x => x.TrackImage)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .Include(x => x.SongLikes)
                    .FirstOrDefaultAsync(x => x.ArtistMemberId == memberId && x.ArtistTrackUploadId == trackId);

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

        public async Task<ServiceResponse<List<ArtistTrackUploadDto>>> GetArtistTracksAsync(string memberId, int? lastTrackId)
        {
            try
            {
                var tracksQuery = _dbContext.ArtistTrackUpload.AsQueryable();

                if (lastTrackId.HasValue) {
                    tracksQuery = tracksQuery.Where(x => x.ArtistMemberId == memberId && x.ArtistTrackUploadId < lastTrackId);
                }
                else
                {
                    tracksQuery = tracksQuery.Where(x => x.ArtistMemberId == memberId);
                }

                var tracks = await tracksQuery
                    .Include(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.TrackData)
                    .Include(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .OrderByDescending(x => x.ArtistTrackUploadId)
                    .Select(x => new ArtistTrackUploadDto
                    {
                        ArtistTrackUploadId = x.ArtistTrackUploadId,
                        TrackName = x.TrackName,
                        TrackDescription = x.TrackDescription,
                        TrackImageUrl = !string.IsNullOrWhiteSpace(x.TrackImageUrl) ? x.TrackImageUrl : x.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage) != null ? x.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage).AccountImageUrl : "",
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
                    tracksQuery = tracksQuery.Where(x => x.MemberId == memberId && x.SongLikeId < lastTrackId);
                }
                else
                {
                    tracksQuery = tracksQuery.Where(x => x.MemberId == memberId);
                }

                var tracks = await tracksQuery
                    .Include(x => x.ArtistTrackUpload)
                    .ThenInclude(x => x.Artist)
                    .ThenInclude(x => x.Images)
                    .Include(x => x.ArtistTrackUpload)
                    .ThenInclude(x => x.TrackData)
                    .Include(x => x.ArtistTrackUpload)
                    .ThenInclude(x => x.Genres)
                    .ThenInclude(x => x.Genre)
                    .OrderByDescending(x => x.SongLikeId)
                    .Select(x => new ArtistTrackUploadDto
                    {
                        ArtistTrackUploadId = x.ArtistTrackUpload.ArtistTrackUploadId,
                        TrackName = x.ArtistTrackUpload.TrackName,
                        TrackDescription = x.ArtistTrackUpload.TrackDescription,
                        TrackImageUrl = !string.IsNullOrWhiteSpace(x.ArtistTrackUpload.TrackImageUrl) ? x.ArtistTrackUpload.TrackImageUrl : x.ArtistTrackUpload.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage) != null ? x.ArtistTrackUpload.Artist.Images.FirstOrDefault(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage).AccountImageUrl : "",
                        ArtistProfile = x.ArtistTrackUpload.Artist.ProfileUrl,
                        ArtistName = x.ArtistTrackUpload.Artist.Name,
                        ArtistMemberId = x.ArtistTrackUpload.Artist.MemberId,
                        TrackUrl = x.ArtistTrackUpload.TrackUrl,
                        Duration = x.ArtistTrackUpload.Duration,
                        UploadDate = x.ArtistTrackUpload.UploadDate,
                        LikeCount = x.ArtistTrackUpload.LikeCount,
                        SongLikeId = x.SongLikeId,
                        Genres = x.ArtistTrackUpload.Genres.Select(genre => new GenreDto
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

        public async Task<ServiceResponse> UnlikeArtistTrackAsync(string memberId, int songLikeId)
        {
            try
            {
                var songLike = await _dbContext.SongLike.Include(x => x.ArtistTrackUpload).FirstOrDefaultAsync(x => x.MemberId == memberId && x.SongLikeId == songLikeId);

                if (songLike is null)
                    return new ServiceResponse(HttpStatusCode.InternalServerError);

                _dbContext.SongLike.Remove(songLike);

                await _dbContext.SaveChangesAsync();

                if (songLike.ArtistTrackUpload != null)
                {
                    bool saveFailed = false;
                    do
                    {
                        songLike.ArtistTrackUpload.LikeCount--;

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
                    GenreId = x.GenreId
                }).ToList();

                if (newTrackImage != null)
                {
                    await _dbContext.ArtistTrackImageFileContent.AddAsync(newTrackImage);

                    if (track.TrackImage != null)
                    {
                        _dbContext.ArtistTrackImageFileContent.Remove(track.TrackImage);
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
