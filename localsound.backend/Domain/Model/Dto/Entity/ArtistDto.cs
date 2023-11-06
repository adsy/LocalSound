using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class ArtistDto : IAppUserDto
    {
        public string MemberId { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? SoundcloudUrl { get; set; }
        public string? SpotifyUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public CustomerTypeEnum CustomerType { get; set; }
        public string? AboutSection { get; set; }
        public bool EmailConfirmed { get; set; }

        public List<GenreDto> Genres { get; set; }
        public List<EquipmentDto> Equipment { get; set; }
        public List<EventTypeDto> EventTypes { get; set; }
        public List<AccountImageDto> Images { get; set; }
        public List<UserSummaryDto> Following { get; set; } = new List<UserSummaryDto>();
        public List<UserSummaryDto> Followers { get; set; } = new List<UserSummaryDto>();
    }
}
