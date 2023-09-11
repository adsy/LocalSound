using localsound.backend.api.Commands.Token;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Repositories;
using MediatR;

namespace localsound.backend.api.Handlers.Token
{
    public class TokenHandler : IRequestHandler<ValidateRefreshTokenCommand, ServiceResponse<TokenDto>>,
        IRequestHandler<ConfirmEmailTokenCommand, ServiceResponse<LoginResponseDto>>,
        IRequestHandler<ResendConfirmEmailTokenCommand, ServiceResponse>
    {
        private ITokenRepository _tokenRepository;

        public TokenHandler(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<ServiceResponse<TokenDto>> Handle(ValidateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _tokenRepository.ValidateRefreshToken(request.AccessToken, request.RefreshToken);
        }

        public async Task<ServiceResponse<LoginResponseDto>> Handle(ConfirmEmailTokenCommand request, CancellationToken cancellationToken)
        {
            return await _tokenRepository.ConfirmEmailToken(request.EmailToken, request.User);
        }

        public async Task<ServiceResponse> Handle(ResendConfirmEmailTokenCommand request, CancellationToken cancellationToken)
        {
            return await _tokenRepository.ResendConfirmEmailToken(request.User);
        }
    }
}
