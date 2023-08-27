using Microsoft.AspNetCore.Http;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class UpdateArtistDto
    {
        public string? ArtistName { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? AboutSection { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public IFormFile? CoverImage { get; set; }
    }
}
