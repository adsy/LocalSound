using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;
using System.Security.Claims;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITokenRepository
    {
        string CreateToken(List<Claim> claims);
        List<Claim> GetClaims(AppUser user);
        Task<ServiceResponse<TokenDto>> ValidateRefreshToken(string accessToken, string refreshToken);
        Task<string> CreateRefreshToken(AppUser user);
    }
}
