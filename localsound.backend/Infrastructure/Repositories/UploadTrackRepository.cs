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
    public class UploadTrackRepository : IUploadTrackRepository
    {
        public readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<UploadTrackRepository> _logger;

        public UploadTrackRepository(LocalSoundDbContext dbContext, ILogger<UploadTrackRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<ArtistTrackUpload>> AddArtistTrackToDbAsync(Guid userId, Guid trackId, Guid genreId, string trackName, string trackDescription, string trackFileExt, string trackLocation, Guid trackImageId, string trackImageFileExt, string trackImageLocation)
        {
            try
            {
                // create track file content
                var trackContent = new FileContent
                {
                    FileContentId = Guid.NewGuid(),
                    FileExtensionType = trackFileExt,
                    FileLocation = trackLocation
                };

                // create track image file content
                var trackImageContent = new FileContent
                {
                    FileContentId = trackImageId,
                    FileExtensionType = trackImageFileExt,
                    FileLocation = trackImageLocation
                };

                var artistTrack = new ArtistTrackUpload
                {
                    ArtistTrackUploadId = trackId,
                    TrackData = trackContent,
                    AppUserId = userId,
                    TrackName = trackName,
                    TrackDescription = trackDescription,
                    TrackImage = trackImageContent,
                    GenreId = genreId
                };

                var result = await _dbContext.ArtistTrackUpload.AddAsync(artistTrack);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.OK)
                {
                    ReturnData = result.Entity
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(UploadTrackRepository)} - {nameof(AddArtistTrackToDbAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.InternalServerError, "There was an error while uploading your track, please try again.");
            }
        }

        public async Task<ServiceResponse> DeletePartialTrackChunksAysnc(Guid partialTrackId)
        {
            try
            {
                var chunks = await _dbContext.ArtistTrackChunk.Where(x => x.PartialTrackId == partialTrackId).ToListAsync();

                _dbContext.RemoveRange(chunks);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackRepository)} - {nameof(DeletePartialTrackChunksAysnc)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<ArtistTrackUpload>> GetArtistTrackAsync(Guid trackId)
        {
            try
            {
                var track = await _dbContext.ArtistTrackUpload
                    .Include(x => x.TrackData)
                    .FirstOrDefaultAsync(x => x.ArtistTrackUploadId == trackId);

                if (track == null)
                    return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.InternalServerError);

                return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.OK)
                {
                    ReturnData = track
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackRepository)} - {nameof(GetArtistTrackAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<ArtistTrackUpload>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<List<ArtistTrackChunkDto>>> GetPartialTrackChunksAsync(Guid partialTrackId)
        {
            try
            {
                var chunkList = await _dbContext.ArtistTrackChunk
                    .Where(x => x.PartialTrackId == partialTrackId)
                    .Select(x => new ArtistTrackChunkDto
                    {
                        ChunkId = x.ChunkId,
                        AppUserId = x.AppUserId,
                        FileContentId = x.FileContentId,
                        FileLocation = x.FileContent.FileLocation,
                        PartialTrackId = partialTrackId
                    })
                    .OrderBy(x => x.ChunkId)
                    .ToListAsync();

                if (chunkList == null || !chunkList.Any())
                {
                    return new ServiceResponse<List<ArtistTrackChunkDto>>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<List<ArtistTrackChunkDto>>(HttpStatusCode.OK)
                {
                    ReturnData = chunkList
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackRepository)} - {nameof(GetPartialTrackChunksAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistTrackChunkDto>>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse> SetTrackReadyAsync(Guid trackId)
        {
            try
            {
                var track = await _dbContext.ArtistTrackUpload.FirstOrDefaultAsync(x => x.ArtistTrackUploadId == trackId);

                if (track == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError);
                }

                track.TrackReady = true;

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackRepository)} - {nameof(SetTrackReadyAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<ArtistTrackChunk>> UploadTrackChunkAsync(string fileLocation, Guid partialTrackId, Guid userId, int chunkId)
        {
            try
            {
                var fileContentId = Guid.NewGuid();

                // create new file content
                var fileContent = new FileContent
                {
                    FileContentId = fileContentId,
                    FileExtensionType = "chunk",
                    FileLocation = fileLocation + $"/{fileContentId}"
                };

                var trackChunk = new ArtistTrackChunk
                {
                    FileContent = fileContent,
                    PartialTrackId = partialTrackId,
                    ChunkId = chunkId,
                    AppUserId = userId
                };

                var result = await _dbContext.ArtistTrackChunk.AddAsync(trackChunk);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse<ArtistTrackChunk>(HttpStatusCode.OK)
                {
                    ReturnData = result.Entity
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(UploadTrackRepository)} - {nameof(UploadTrackChunkAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<ArtistTrackChunk>(HttpStatusCode.InternalServerError, "There was an error while uploading your track, please try again.");
            }
        }
    }
}
