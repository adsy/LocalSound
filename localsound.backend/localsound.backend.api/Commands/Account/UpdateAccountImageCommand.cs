using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using MediatR;

namespace localsound.backend.api.Commands.Account
{
    public class UpdateAccountImageCommand : IRequest<ServiceResponse<string>>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public IFormFile Photo { get; set; }
        public AccountImageTypeEnum ImageType { get; set; }
        public string FileExt { get; set; }
    }
}