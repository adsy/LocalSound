using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IAccountService
    {
        Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginSubmissionDto loginData);
        Task<ServiceResponse<LoginResponseDto>> RegisterAsync(RegisterSubmissionDto registrationDetails);
    }
}
