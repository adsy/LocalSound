namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class BookingDto
    {
        public Guid BookingId { get; set; }
        public string BookerId { get; set; }
        public string BookerName { get; set; }
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string PackageName { get; set; }
        public string PackagePrice { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal BookingLength { get; set; }
        public string BookingAddress { get; set; }
        public string EventType { get; set; }
        public string BookingDescription { get; set; }
        public bool? BookingConfirmed { get; set; }
        public List<EquipmentDto> PackageEquipment { get; set; }
    }
}
