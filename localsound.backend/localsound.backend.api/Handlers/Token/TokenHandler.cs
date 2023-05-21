using localsound.backend.api.Commands.Token;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using MediatR;

namespace localsound.backend.api.Handlers.Token
{
    public class TokenHandler : IRequestHandler<ValidateRefreshTokenCommand, ServiceResponse<TokenDto>>
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
    }
}
