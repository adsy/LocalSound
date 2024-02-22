using System.Threading.Tasks;

namespace localsound.CoreUpdates.Services
{
    public interface ITrackService
    {
        Task<bool> DeleteArtistTrack(int artistTrackId, string artistMemberId);
        Task<bool> DeleteArtistTrackImage(int artistTrackId, string artistMemberId);
    }
}
