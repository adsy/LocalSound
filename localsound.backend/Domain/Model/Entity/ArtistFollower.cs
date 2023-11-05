using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class ArtistFollower
    {
        [ForeignKey("Artist")]
        public Guid ArtistId { get; set; }
        [ForeignKey("Follower")]
        public Guid FollowerId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual AppUser Follower { get; set; }
    }
}
