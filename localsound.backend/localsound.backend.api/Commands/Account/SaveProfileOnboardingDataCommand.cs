using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Account
{
    public class SaveProfileOnboardingDataCommand : IRequest<ServiceResponse>
    {
        public Guid UserId { get; set; }
        public string MemberId { get; set; }
        public SaveOnboardingDataDto OnboardingData { get; set; }
    }
}
