using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model;
using MediatR;
using System.Security.Claims;

namespace localsound.backend.api.Commands.Token
{
    public class ConfirmEmailTokenCommand : IRequest<ServiceResponse<LoginResponseDto>>
    {
        public string EmailToken { get; set; }
        public ClaimsPrincipal User { get; set; }
    }
}
