using localsound.backend.Domain.Model.Interfaces;

namespace localsound.backend.Domain.Model.Dto
{
    public class LoginResponseDto
    {
        public IAppUserDto UserDetails { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
