﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace localsound.backend.Domain.Model.Entity
{
    public class Artist
    {
        public Artist(Guid appUserId, AppUser user, string name, string profileUrl, string address, string phoneNumber, string? soundcloudUrl, string? spotifyUrl, string? youtubeUrl, string? aboutSection)
        {
            AppUserId = appUserId;
            User = user;
            Name = name;
            ProfileUrl = profileUrl;
            Address = address;
            PhoneNumber = phoneNumber;
            SoundcloudUrl = soundcloudUrl;
            SpotifyUrl = spotifyUrl;
            YoutubeUrl = youtubeUrl;
            AboutSection = aboutSection;
        }

        [Key]
        [ForeignKey("User")]
        public Guid AppUserId { get; set; }
        public AppUser User { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? AboutSection { get; set; }
    }
}
