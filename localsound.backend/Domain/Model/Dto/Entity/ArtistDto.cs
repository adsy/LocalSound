﻿using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Interfaces;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class ArtistDto : IAppUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string SoundcloudUrl { get; set; }
        public string SpotifyUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
