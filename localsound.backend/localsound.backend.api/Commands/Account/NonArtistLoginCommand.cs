using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Commands.Account
{
    public class NonArtistLoginCommand: IRequest<ServiceResponse<LoginResponseDto>>
    {
        public LoginDataDto UserDetails { get; set; }
    }
}
