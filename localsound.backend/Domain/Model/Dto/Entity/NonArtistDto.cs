using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Interfaces;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class NonArtistDto : IAppUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
