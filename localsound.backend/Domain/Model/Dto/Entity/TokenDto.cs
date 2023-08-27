namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class TokenDto
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string UserId { get; set; }
    }
}
