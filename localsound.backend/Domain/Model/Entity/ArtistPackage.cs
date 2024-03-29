﻿using System.ComponentModel.DataAnnotations.Schema;

namespace localsound.backend.Domain.Model.Entity
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

        public virtual Account Artist { get; set; }
        public virtual ICollection<ArtistPackageEquipment> Equipment { get; set; }
        public virtual ICollection<ArtistPackageImage> PackagePhotos { get; set; }
        public virtual ICollection<ArtistBooking> RelatedBookings { get; set; }

        public ArtistPackage UpdateDetails(string name, string description, string price)
        {
            PackageName = name;
            PackageDescription = description;
            PackagePrice = price;

            return this;
        }

        public ArtistPackage UpdateEquipment (ICollection<ArtistPackageEquipment> equipment)
        {
            Equipment = equipment;

            return this;
        }

        public ArtistPackage UpdatePhotos (ICollection<ArtistPackageImage> photos)
        {
            foreach(var photo in photos)
            {
                PackagePhotos.Add(photo);
            }
            return this;
        }

        public ArtistPackage RemovePhotos(ICollection<ArtistPackageImage> photos)
        {
            foreach (var photo in photos)
            {
                PackagePhotos.Remove(photo);
            }
            return this;
        }
    }
}
