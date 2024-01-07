using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Domain.Model.Dto.Response
{
    public class BookingListResponse
    {
        public List<BookingDto> Bookings { get; set; }
        public bool CanLoadMore { get; set; }
    }
}
