using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class ArtistDto : IAppUserDto
    {
        public ArtistDto(string memberId, string name, string profileUrl, string email, string address, string phoneNumber, string? soundcloudUrl, string? spotifyUrl, string? youtubeUrl, CustomerType customerType)
        {
            MemberId = memberId;
            Name = name;
            ProfileUrl = profileUrl;
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
            SoundcloudUrl = soundcloudUrl;
            SpotifyUrl = spotifyUrl;
            YoutubeUrl = youtubeUrl;
            CustomerType = customerType;
        }

        public string MemberId { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
