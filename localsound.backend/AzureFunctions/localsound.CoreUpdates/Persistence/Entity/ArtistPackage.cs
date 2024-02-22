using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace localsound.CoreUpdates.Persistence.Entity
{
    public class ArtistPackage
    {
        public Guid ArtistPackageId { get; set; }
        [ForeignKey("Artist")]
        public Guid AppUserId { get; set; }
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public string PackagePrice { get; set; }
        public bool IsAvailable { get; set; }

        public virtual ICollection<ArtistPackageImage> PackagePhotos { get; set; }
        public virtual ICollection<ArtistBooking> RelatedBookings { get; set; }
    }
}
