using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Interfaces.Entity;
using MediatR;
using System.Security.Claims;

namespace localsound.backend.api.Queries.Account
{
    public class CheckCurrentUserTokenQuery : IRequest<ServiceResponse<IAppUserDto>>
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
