using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
{
    public class AccountGenre
    {
        [ForeignKey("Artist")]
        public Guid AppUserId { get; set; }
        
        [ForeignKey("Genre")]
        public Guid GenreId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Account Artist { get; set; }
    }
}
