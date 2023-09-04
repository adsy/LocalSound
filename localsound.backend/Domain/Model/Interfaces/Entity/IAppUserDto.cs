using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Domain.Model.Interfaces.Entity
{
    public interface IAppUserDto
    {
        public string MemberId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public CustomerTypeEnum CustomerType { get; set; }
        //public bool EmailConfirmed { get; set; }

        public List<AccountImageDto> Images { get; set; }
    }
}
