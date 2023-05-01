using localsound.backend.Domain.Enum;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class RegistrationDto
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string SoundcloudUrl { get; set; }
        public string SpotifyUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
