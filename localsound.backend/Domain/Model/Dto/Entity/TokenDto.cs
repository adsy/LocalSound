namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class TokenDto
    {
        public TokenDto(string refreshToken, string accessToken, string userId)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
            UserId = userId;
        }

        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string UserId { get; set; }
    }
}
