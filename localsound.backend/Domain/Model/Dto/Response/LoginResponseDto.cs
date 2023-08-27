using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Domain.Model.Dto.Response
{
    public class LoginResponseDto
    {
        public LoginResponseDto(IAppUserDto userDetails, string refreshToken, string accessToken)
        {
            UserDetails = userDetails;
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }

        public IAppUserDto UserDetails { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
