using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Interfaces.Entity;
using MediatR;

namespace localsound.backend.api.Queries.Account
{
    public class GetProfileDataQuery : IRequest<ServiceResponse<IAppUserDto>>
    {
        public string ProfileUrl { get; set; }
    }
}
