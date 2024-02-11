using localsound.backend.api.Commands.Account;
using localsound.backend.api.Commands.Artist;
using localsound.backend.api.Queries.Account;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Services;
using localsound.backend.Infrastructure.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Account
{
    public class AccountHandler : IRequestHandler<LoginCommand, ServiceResponse<LoginResponseDto>>,
        IRequestHandler<RegisterCommand, ServiceResponse<LoginResponseDto>>,
        IRequestHandler<GetProfileDataQuery, ServiceResponse<IAppUserDto>>,
        IRequestHandler<UpdateAccountImageCommand, ServiceResponse<string>>,
        IRequestHandler<GetAccountImageQuery, ServiceResponse<AccountImageDto>>,
        IRequestHandler<CheckCurrentUserTokenQuery, ServiceResponse<IAppUserDto>>,
        IRequestHandler<GetProfileFollowerDataQuery, ServiceResponse<FollowerListResponseDto>>,
        IRequestHandler<SaveProfileOnboardingDataCommand, ServiceResponse>, 
        IRequestHandler<UpdateArtistPersonalDetailsCommand, ServiceResponse>,
        IRequestHandler<UpdateArtistProfileDetailsCommand, ServiceResponse>,
        IRequestHandler<UpdateArtistFollowerCommand, ServiceResponse>
    {
        private readonly IAccountService _accountService;

        public AccountHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        public async Task<ServiceResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.LoginAsync(request.LoginDetails);
        }

        public async Task<ServiceResponse<LoginResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RegisterAsync(request.RegistrationDetails);
        }

        public async Task<ServiceResponse<IAppUserDto>> Handle(GetProfileDataQuery request, CancellationToken cancellationToken)
        {
            return await _accountService.GetProfileDataAsync(request.ProfileUrl, request.CurrentUser, cancellationToken);
        }

        public async Task<ServiceResponse<string>> Handle(UpdateAccountImageCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.UpdateAccountImageAsync(request.UserId, request.MemberId, request.Photo, request.ImageType, request.FileExt);
        }

        public async Task<ServiceResponse<AccountImageDto>> Handle(GetAccountImageQuery request, CancellationToken cancellationToken)
        {
            return await _accountService.GetAccountImageAsync(request.UserId, request.MemberId, request.ImageType);
        }

        public async Task<ServiceResponse<IAppUserDto>> Handle(CheckCurrentUserTokenQuery request, CancellationToken cancellationToken)
        {
            return await _accountService.CheckCurrentUserTokenAsync(request.ClaimsPrincipal);
        }

        public async Task<ServiceResponse<FollowerListResponseDto>> Handle(GetProfileFollowerDataQuery request, CancellationToken cancellationToken)
        {
            return await _accountService.GetProfileFollowerDataAsync(request.MemberId, request.Page, request.RetrieveFollowing, cancellationToken);
        }

        public async Task<ServiceResponse> Handle(SaveProfileOnboardingDataCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.SaveOnboardingDataAsync(request.UserId, request.MemberId, request.OnboardingData);
        }
        public async Task<ServiceResponse> Handle(UpdateArtistPersonalDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.UpdateArtistPersonalDetails(request.UserId, request.MemberId, request.UpdateArtistDto);
        }

        public async Task<ServiceResponse> Handle(UpdateArtistProfileDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.UpdateArtistProfileDetails(request.UserId, request.MemberId, request.UpdateArtistDto);
        }

        public async Task<ServiceResponse> Handle(UpdateArtistFollowerCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.UpdateArtistFollower(request.UserId, request.MemberId, request.ArtistId, request.StartFollowing);
        }
    }
}
