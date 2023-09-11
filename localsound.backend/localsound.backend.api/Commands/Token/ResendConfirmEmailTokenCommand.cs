using localsound.backend.Domain.Model;
using MediatR;
using System.Security.Claims;

namespace localsound.backend.api.Commands.Token
{
    public class ResendConfirmEmailTokenCommand : IRequest<ServiceResponse>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
