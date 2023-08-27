using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Interfaces.Entity;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class NonArtistDto : IAppUserDto
    {
        public NonArtistDto(string memberId, string email, string address, string phoneNumber, string firstName, string lastName, CustomerType customerType)
        {
            MemberId = memberId;
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            CustomerType = customerType;
        }
        public string MemberId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
