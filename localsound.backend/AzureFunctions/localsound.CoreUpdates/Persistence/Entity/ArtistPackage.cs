using System.ComponentModel.DataAnnotations.Schema;
using System;

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
    }
}
