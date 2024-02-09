using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Entity;
using System.Security.Claims;

namespace localsound.backend.Infrastructure.Interface.Repositories
{
    public interface ITokenRepository
    {
        string CreateToken(List<Claim> claims);
        List<Claim> GetClaims(AppUser user, CustomerTypeEnum customerType);
        Task<ServiceResponse<TokenDto>> ValidateRefreshToken(string accessToken, string refreshToken);
        Task<string> CreateRefreshToken(AppUser user);
        Task<ServiceResponse<LoginResponseDto>> ConfirmEmailToken(string emailToken, ClaimsPrincipal claims);
        Task<ServiceResponse> ResendConfirmEmailToken(ClaimsPrincipal claims);
    }
}
