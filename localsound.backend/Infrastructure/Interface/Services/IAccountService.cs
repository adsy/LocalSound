using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Interfaces.Entity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IAccountService
    {
        Task<ServiceResponse<IAppUserDto>> GetProfileDataAsync(string profileUrl, Guid? currentUser, CancellationToken cancellationToken);
        Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginSubmissionDto loginData);
        Task<ServiceResponse<LoginResponseDto>> RegisterAsync(RegisterSubmissionDto registrationDetails);
        Task<ServiceResponse> UpdateAccountImage(Guid userId, string memberId, IFormFile photo, AccountImageTypeEnum imageType);
        Task<ServiceResponse<AccountImageDto>> GetAccountImage(Guid userId, string memberId, AccountImageTypeEnum imageType);
        Task<ServiceResponse<IAppUserDto>> CheckCurrentUserToken(ClaimsPrincipal claimsPrincipal);
        Task<ServiceResponse<FollowerListResponseDto>> GetProfileFollowersAsync(string memberId, int page, CancellationToken cancellationToken);
    }
}
