namespace localsound.backend.Domain.Model.Entity
{
    public class NonArtist
    {
        public string AppUserId { get; set; }
        public AppUser User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
