using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Helper;

namespace localsound.backend.Infrastructure.Helper
{
    public class SearchHelper : ISearchHelper
    {
        public int SongLikeBinarySearch(List<SongLike> arr, int target)
        {
            int left = 0;
            int right = arr.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                int comparisonResult = target.CompareTo(arr[mid].ArtistTrackId);

                if (comparisonResult == 0)
                    return arr[mid].SongLikeId;

                if (comparisonResult > 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1;
        }
    }
}
