using localsound.backend.api.Commands.Account;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Account
{
    public class AccountHandler : IRequestHandler<LoginCommand, ServiceResponse<LoginResponseDto>>,
        IRequestHandler<RegisterCommand, ServiceResponse<LoginResponseDto>>
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
    }
}
