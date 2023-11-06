namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class UserSummaryDto
    {
        public string MemberId { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }

        public List<AccountImageDto> Images { get; set; }
    }
}
