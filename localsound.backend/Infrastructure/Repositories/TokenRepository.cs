using AutoMapper;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Domain.ModelAdaptor;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace localsound.backend.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettingsAdaptor _settings;
        private readonly ILogger<TokenRepository> _logger;
        private readonly LocalSoundDbContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IEmailRepository _emailRepository;

        public TokenRepository(UserManager<AppUser> userManager,
            IOptions<JwtSettingsAdaptor> settings,
            ILogger<TokenRepository> logger,
            LocalSoundDbContext context,
            IAccountRepository accountRepository,
            IMapper mapper,
            IEmailRepository emailRepository)
        {
            _userManager = userManager;
            _settings = settings.Value;
            _logger = logger;
            _context = context;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _emailRepository = emailRepository;
        }

        // Create new Access Token
        public string CreateToken(List<Claim> claims)
        {
            // create key and use it to create credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // create securityTokenDescriptor with the claims and credentials + expiry date
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = credentials
            };

            // create token handler which returns token and writes it as response

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public List<Claim> GetClaims(AppUser user)
        {
            // create claims for user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            if (user.CustomerType == CustomerTypeEnum.Artist)
            {
                claims.Add(new Claim(ClaimTypes.Role, CustomerTypeEnum.Artist.ToString()));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, CustomerTypeEnum.Artist.ToString()));
            }

            return claims;
        }

        public async Task<ServiceResponse<TokenDto>> ValidateRefreshToken(string accessToken, string refreshToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
                    return new ServiceResponse<TokenDto>(HttpStatusCode.Unauthorized);

                var claims = GetPrincipalFromExpiredToken(accessToken);

                var parsedGuidResult = Guid.TryParse(claims.FindFirstValue(ClaimTypes.NameIdentifier), out var id);

                if (!parsedGuidResult)
                {
                    var unAuthedErrorMessage = $"{nameof(TokenRepository)} - {nameof(ValidateRefreshToken)} - ClaimsPrincipal does not contain user Id";
                    _logger.LogError(unAuthedErrorMessage);

                    return new ServiceResponse<TokenDto>(HttpStatusCode.Unauthorized);
                }

                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null)
                {
                    return new ServiceResponse<TokenDto>(HttpStatusCode.Unauthorized);
                }

                var token = await _context.UserTokens.FirstOrDefaultAsync(o => o.UserId == id);

                if (token == null || token.Value != refreshToken || token.ExpirationDate <= DateTime.Now.ToLocalTime())
                {
                    await _userManager.RemoveAuthenticationTokenAsync(user, "RefreshTokenProviderExtension", "RefreshToken");
                    return new ServiceResponse<TokenDto>(HttpStatusCode.Unauthorized);
                }

                //create new access token
                var newAccessToken = CreateToken(claims.Claims.ToList());
                var newRefreshToken = await CreateRefreshToken(user);

                if (string.IsNullOrWhiteSpace(newAccessToken) || string.IsNullOrWhiteSpace(newRefreshToken))
                {
                    return new ServiceResponse<TokenDto>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<TokenDto>(HttpStatusCode.OK)
                {
                    ReturnData = new TokenDto
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken,
                        UserId = user.Id.ToString()
                    }
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(TokenRepository)} - {nameof(ValidateRefreshToken)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<TokenDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = e.Message
                };
            }
        }
        public async Task<string> CreateRefreshToken(AppUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "RefreshTokenProviderExtension", "RefreshToken");

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "RefreshTokenProviderExtension", "RefreshToken");

            await _userManager.SetAuthenticationTokenAsync(user, "RefreshTokenProviderExtension", "RefreshToken", newRefreshToken);

            return newRefreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<ServiceResponse<LoginResponseDto>> ConfirmEmailToken(string emailToken, ClaimsPrincipal claims)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(emailToken) || claims == null)
                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.Unauthorized);

                var id = Guid.TryParse(claims.FindFirstValue(ClaimTypes.NameIdentifier), out Guid parsedId) ? parsedId : Guid.Empty;

                if (id == Guid.Empty)
                {
                    var unAuthedErrorMessage = $"{nameof(TokenRepository)} - {nameof(ConfirmEmailToken)} - ClaimsPrincipal does not contain user Id";
                    _logger.LogError(unAuthedErrorMessage);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.Unauthorized);
                }

                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null)
                {
                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.BadRequest);
                }

                // validate refresh token
                var result = await _userManager.ConfirmEmailAsync(user, emailToken);

                if (!result.Succeeded)
                {
                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.BadRequest, "The email confirmation token entered is incorrect.");
                }

                var returnUser = null as IAppUserDto;

                // Create store or user based on customer type
                if (user.CustomerType == CustomerTypeEnum.Artist)
                {
                    var customer = await _accountRepository.GetArtistFromDbAsync(id);

                    if (!customer.IsSuccessStatusCode)
                        return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError);

                    returnUser = _mapper.Map<ArtistDto>(customer.ReturnData);
                    returnUser.Email = user.Email;
                }
                else
                {
                    var store = await _accountRepository.GetNonArtistFromDbAsync(id);

                    if (!store.IsSuccessStatusCode)
                        return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError);

                    returnUser = _mapper.Map<NonArtistDto>(store.ReturnData);
                    returnUser.Email = user.Email;
                }

                var accessToken = CreateToken(GetClaims(user));
                var refreshToken = await CreateRefreshToken(user);

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.OK)
                {
                    ReturnData = new LoginResponseDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        UserDetails = returnUser
                    }
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(TokenRepository)} - {nameof(ConfirmEmailToken)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = e.Message
                };
            }
        }

        public async Task<ServiceResponse> ResendConfirmEmailToken(ClaimsPrincipal claims)
        {
            try
            {
                var id = Guid.TryParse(claims.FindFirstValue(ClaimTypes.NameIdentifier), out Guid parsedId) ? parsedId : Guid.Empty;

                if (id == Guid.Empty)
                {
                    var unAuthedErrorMessage = $"{nameof(TokenRepository)} - {nameof(ConfirmEmailToken)} - ClaimsPrincipal does not contain user Id";
                    _logger.LogError(unAuthedErrorMessage);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.Unauthorized);
                }

                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                    return new ServiceResponse(HttpStatusCode.BadRequest);

                // Send email address confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailRepository.SendConfirmEmailTokenMessageAsync(token, user.Email);

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(TokenRepository)} - {nameof(ResendConfirmEmailToken)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = e.Message
                };
            }
        }
    }
}
