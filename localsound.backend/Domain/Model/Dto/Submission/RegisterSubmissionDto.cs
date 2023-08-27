using localsound.backend.Domain.Enum;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class RegisterSubmissionDto
    {
        public CustomerType CustomerType { get; set; }
        public RegistrationDto RegistrationDto { get; set; }
    }
}
