namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class CreateBookingDto
    {
        public string ArtistId { get; set; }
        public Guid PackageId { get; set; }
        public string BookingDescription { get; set; }
        public string BookingAddress { get; set; }
        public decimal BookingLength { get; set; }
        public Guid EventTypeId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
