using localsound.backend.Domain.Enum;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class RegisterSubmissionDto
    {
        public RegisterSubmissionDto(CustomerType customerType, RegistrationDto registrationDto)
        {
            CustomerType = customerType;
            RegistrationDto = registrationDto;
        }

        public CustomerType CustomerType { get; set; }
        public RegistrationDto RegistrationDto { get; set; }
    }
}
