using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
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

        public async Task<ServiceResponse<ArtistTrackUpload>> AddArtistTrackToDbAsync(Guid userId, Guid trackId, string trackName, string trackDescription, string trackFileExt, string trackLocation, Guid trackImageId, string trackImageFileExt, string trackImageLocation)
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
                    TrackImage = trackImageContent
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
