using AutoMapper;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManger;

        public AccountService(
            IAccountRepository accountRepository, 
            ITokenRepository tokenRepository,
            IMapper mapper,
            ILogger<AccountService> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger
            )
        {
            _accountRepository = accountRepository;
            _tokenRepository = tokenRepository;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManger = signInManger;
        }

        public async Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginDataDto loginData)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginData.Email);

                if (user == null)
                {
                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.BadRequest)
                    {
                        ServiceResponseMessage = "The email or password you entered is incorrect..."
                    };
                }

                var passwordSuccessful = await _userManager.CheckPasswordAsync(user, loginData.Password);

                if (!passwordSuccessful)
                {
                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.BadRequest)
                    {
                        ServiceResponseMessage = "The email or password you entered is incorrect..."
                    };
                }

                var accessToken = _tokenRepository.CreateToken(_tokenRepository.GetClaims(user));

                if (user.CustomerType == CustomerType.Artist)
                {
                    var artist = await _accountRepository.GetArtistFromDbAsync(user.Id);

                    var returnDto = _mapper.Map<ArtistDto>(artist);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.OK)
                    {
                        ReturnData = new LoginResponseDto
                        {
                            UserDetails = returnDto,
                            AccessToken = accessToken
                        }
                    };
                }
                else
                {
                    var nonArtist = await _accountRepository.GetNonArtistFromDbAsync(user.Id);

                    var returnDto = _mapper.Map<NonArtistDto>(nonArtist);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.OK)
                    {
                        ReturnData = new LoginResponseDto
                        {
                            UserDetails = returnDto,
                            AccessToken = accessToken
                        }
                    };
                }
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(LoginAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                }; ;
            }
        }
    }
}
