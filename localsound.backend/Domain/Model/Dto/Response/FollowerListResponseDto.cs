using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Domain.Model.Dto.Response
{
    public class FollowerListResponseDto
    {
        public List<UserSummaryDto> Followers { get; set; }
        public bool CanLoadMore { get; set; }
    }
}
