using localsound.backend.Infrastructure.Interface.Helper;

namespace localsound.backend.Infrastructure.Helper
{
    public class SearchHelper : ISearchHelper
    {
        public int GuidBinarySearch(List<Guid> arr, Guid target)
        {
            int left = 0;
            int right = arr.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                int comparisonResult = target.CompareTo(arr[mid]);

                if (comparisonResult == 0)
                    return mid;

                if (comparisonResult > 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1;
        }
    }
}
