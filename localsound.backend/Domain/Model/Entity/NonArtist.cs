using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Domain.Model.Entity
{
    public class NonArtist : CustomerType
    {
        public Guid AppUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfileUrl { get; set; }

        public virtual AppUser User { get; set; }
        //public virtual ArtistFollower ArtistsFollowing { get; set; }
    }
}
