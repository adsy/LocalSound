using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace localsound.backend.Domain.Model.Entity
{
    public class Artist
    {
        [Key]
        [ForeignKey("User")]
        public Guid AppUserId { get; set; }
        public AppUser User { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string SoundcloudUrl { get; set; }
        public string SpotifyUrl { get; set; }
        public string YoutubeUrl { get; set; }
    }
}
