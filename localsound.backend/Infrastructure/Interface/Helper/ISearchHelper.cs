using localsound.backend.Domain.Model.Entity;

namespace localsound.backend.Infrastructure.Interface.Helper
{
    public interface ISearchHelper
    {
        int SongLikeBinarySearch(List<SongLike> arr, int target);
    }
}
