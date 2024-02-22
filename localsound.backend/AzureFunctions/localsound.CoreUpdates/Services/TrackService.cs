using localsound.CoreUpdates.Repository;
using System.Linq;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace localsound.CoreUpdates.Services
{
    public class TrackService : ITrackService
    {
        private readonly IDbOperationRepository _dbOperationRepository;
        private readonly IBlobRepository _blobRepository;

        public TrackService(IDbOperationRepository dbOperationRepository, IBlobRepository blobRepository)
        {
            _dbOperationRepository = dbOperationRepository;
            _blobRepository = blobRepository;
        }

        public async Task<bool> DeleteArtistTrack(int artistTrackId, string artistMemberId)
        {
            var artistTrackFileLocations = await _dbOperationRepository.GetArtistTrackLocations(artistTrackId, artistMemberId);

            if (artistTrackFileLocations != null && artistTrackFileLocations.Any())
            {
                foreach (var location in artistTrackFileLocations)
                {
                    var result = await _blobRepository.DeleteEntityFromStorage(location);

                    if (!result)
                        return false;
                }
            }

            var dbDeleteResult = await _dbOperationRepository.DeleteArtistTrackAsync(artistTrackId, artistMemberId);

            if (!dbDeleteResult)
                return dbDeleteResult;

            return true;
        }

        public async Task<bool> DeleteArtistTrackImage(int artistTrackId, string artistMemberId)
        {
            var artistTrackImageDto = await _dbOperationRepository.GetArtistTrackImageLocation(artistTrackId, artistMemberId);

            if (artistTrackImageDto != null)
            {
                var result = await _blobRepository.DeleteEntityFromStorage(artistTrackImageDto.ArtistTrackImageFileLocation);

                if (!result)
                    return false;
            }

            var dbDeleteResult = await _dbOperationRepository.DeleteArtistTrackImageAsync(artistTrackImageDto.ArtistTrackImageId);

            if (!dbDeleteResult)
                return dbDeleteResult;

            return true;
        }
    }
}
