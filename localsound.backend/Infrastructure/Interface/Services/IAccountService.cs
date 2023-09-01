using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Interfaces.Entity;
using Microsoft.AspNetCore.Http;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IAccountService
    {
        Task<ServiceResponse<IAppUserDto>> GetProfileDataAsync(string profileUrl, CancellationToken cancellationToken);
        Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginSubmissionDto loginData);
        Task<ServiceResponse<LoginResponseDto>> RegisterAsync(RegisterSubmissionDto registrationDetails);
        Task<ServiceResponse<string>> UpdateProfileImage(Guid userId, string memberId, IFormFile photo);
    }
}
