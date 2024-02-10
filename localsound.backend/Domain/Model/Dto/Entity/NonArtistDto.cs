using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Interfaces.Entity;
using System.Text.Json.Serialization;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class NonArtistDto : IAppUserDto
    {
        public string MemberId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CustomerTypeEnum CustomerType { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ProfileUrl { get; set; }
        public bool AccountSetupCompleted { get; set; }

        public List<AccountImageDto> Images { get; set; }
        public int FollowingCount { get; set; }
        public int FollowerCount { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsFollowing { get; set; }
    }
}
