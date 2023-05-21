using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Token
{
    public class ValidateRefreshTokenCommand : IRequest<ServiceResponse<TokenDto>>
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
