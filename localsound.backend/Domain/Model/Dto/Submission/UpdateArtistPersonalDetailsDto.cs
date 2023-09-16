using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class UpdateArtistPersonalDetailsDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? AboutSection { get; set; }
        public string ProfileUrl { get; set; }
    }
}
