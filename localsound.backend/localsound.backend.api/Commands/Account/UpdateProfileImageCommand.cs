using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Account
{
    public class UpdateProfileImageCommand : IRequest<ServiceResponse<string>>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public IFormFile Photo { get; set; }
    }
}