using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using MediatR;

namespace localsound.backend.api.Queries.Account
{
    public class GetProfileFollowersQuery : IRequest<ServiceResponse<FollowerListResponseDto>>
    {
        public string MemberId { get; set; }
        public int Page { get; set; }
    }
}
