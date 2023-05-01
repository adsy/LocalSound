using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto;
using MediatR;

namespace localsound.backend.api.Commands.Account
{
    public class LoginCommand: IRequest<ServiceResponse<LoginResponseDto>>
    {
        public LoginDataDto UserDetails { get; set; }
    }
}
