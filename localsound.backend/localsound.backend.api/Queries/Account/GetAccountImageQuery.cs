using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using MediatR;

namespace localsound.backend.api.Queries.Account
{
    public class GetAccountImageQuery : IRequest<ServiceResponse<AccountImageDto>>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public AccountImageTypeEnum ImageType { get; set; }
    }
}
