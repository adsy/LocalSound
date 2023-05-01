using localsound.backend.Domain.Enum;

namespace localsound.backend.Domain.Model.Interfaces
{
    public interface IAppUserDto
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public CustomerType CustomerType { get; set; }
        //public bool EmailConfirmed { get; set; }
    }
}
