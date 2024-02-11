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
        Task<ServiceResponse<string>> UpdateAccountImageAsync(Guid userId, string memberId, IFormFile photo, AccountImageTypeEnum imageType, string fileExt);
        Task<ServiceResponse<AccountImageDto>> GetAccountImageAsync(Guid userId, string memberId, AccountImageTypeEnum imageType);
        Task<ServiceResponse<IAppUserDto>> CheckCurrentUserTokenAsync(ClaimsPrincipal claimsPrincipal);
        Task<ServiceResponse<FollowerListResponseDto>> GetProfileFollowerDataAsync(string memberId, int page, bool retrieveFollowing, CancellationToken cancellationToken);
        Task<ServiceResponse> SaveOnboardingDataAsync(Guid userId, string memberId, SaveOnboardingDataDto onboardingData);
        Task<ServiceResponse> UpdateArtistPersonalDetails(Guid userId, string memberId, UpdateArtistPersonalDetailsDto updateArtistDto);
        Task<ServiceResponse> UpdateArtistProfileDetails(Guid userId, string memberId, UpdateArtistProfileDetailsDto updateArtistDto);
        Task<ServiceResponse> UpdateArtistFollower(Guid userId, string followerId, string artistId, bool startFollowing);
    }
}
