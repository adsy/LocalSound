﻿using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace localsound.CoreUpdates.Persistence.Entity
{
    public class ArtistBooking
    {
        public int BookingId { get; set; }
        [ForeignKey("Artist")]
        public Guid ArtistId { get; set; }
        [ForeignKey("Booker")]
        public Guid BookerId { get; set; }
        [ForeignKey("Package")]
        public Guid PackageId { get; set; }
        [ForeignKey("EventType")]
        public Guid EventTypeId { get; set; }
        public string BookingDescription { get; set; }
        public string BookingAddress { get; set; }
        public decimal BookingLength { get; set; }
        public DateTime BookingDate { get; set; }
        public bool? BookingConfirmed { get; set; }
        public bool BookingCompleted { get; set; }
    }
}
