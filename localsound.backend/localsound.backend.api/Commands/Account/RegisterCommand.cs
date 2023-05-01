using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Account
{
    public class RegisterCommand : IRequest<ServiceResponse<LoginResponseDto>>
    {
        public RegisterSubmissionDto RegistrationDetails { get; set; }
    }
}
