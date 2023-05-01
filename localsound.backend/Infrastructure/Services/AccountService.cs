using AutoMapper;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginSubmissionDto loginData)
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
                    returnDto.MemberId = user.MemberId;

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
                    returnDto.MemberId = user.MemberId;

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

        public async Task<ServiceResponse<LoginResponseDto>> RegisterAsync(RegisterSubmissionDto registrationDetails)
        {
            try
            {
                var userResponse = await CreateUserAsync(registrationDetails.CustomerType, registrationDetails.RegistrationDto);

                if (!userResponse.IsSuccessStatusCode || userResponse.ReturnData is null)
                {
                    return new ServiceResponse<LoginResponseDto>(userResponse.StatusCode)
                    {
                        ServiceResponseMessage = userResponse.ServiceResponseMessage
                    };
                }

                IAppUserDto userDto = null;

                if (registrationDetails.CustomerType == CustomerType.Artist)
                {
                    userDto = await CreateArtistAsync(registrationDetails.RegistrationDto, userResponse.ReturnData);
                }
                else
                {
                    userDto = await CreateNonArtistAsync(registrationDetails.RegistrationDto, userResponse.ReturnData);
                }

                if (userDto is null)
                {
                    // If we cannot add into the user database, delete the user from the aspUsers table so they can resubmit
                    await _userManager.DeleteAsync(userResponse.ReturnData);

                    var message = $"{nameof(AccountService)} - {nameof(RegisterAsync)} - User account was created but could not be stored in the customer db using email:{registrationDetails.RegistrationDto.Email}";
                    _logger.LogError(message);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError, "Something went wrong while creating your account, please try again.");
                }

                userDto.MemberId = userResponse.ReturnData.MemberId;

                var accessToken = _tokenRepository.CreateToken(_tokenRepository.GetClaims(userResponse.ReturnData));

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.OK)
                {
                    ReturnData = new LoginResponseDto
                    {
                        UserDetails = userDto,
                        AccessToken = accessToken
                    }
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(RegisterAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                }; ;
            }
        }

        private async Task<IAppUserDto> CreateNonArtistAsync(RegistrationDto registrationDto, AppUser user)
        {
            var newNonArtist = new NonArtist
            {
                Address = registrationDto.Address,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                PhoneNumber = registrationDto.PhoneNumber,
                User = user,
                AppUserId = user.Id,
            };

            var customerDbResult = await _accountRepository.AddNonArtistToDbAsync(newNonArtist);

            if (customerDbResult.IsSuccessStatusCode)
            {
                var returnUser = _mapper.Map<NonArtistDto>(newNonArtist);

                return returnUser;
            }

            return null;
        }

        private async Task<IAppUserDto> CreateArtistAsync(RegistrationDto registrationDto, AppUser user)
        {
            var newArtist = new Artist
            {
                Address = registrationDto.Address,
                Name = registrationDto.Name,
                PhoneNumber = registrationDto.PhoneNumber,
                User = user,
                AppUserId = user.Id,
                YoutubeUrl = registrationDto.YoutubeUrl,
                SpotifyUrl = registrationDto.SpotifyUrl,
                SoundcloudUrl = registrationDto.SoundcloudUrl
            };

            var customerDbResult = await _accountRepository.AddArtistToDbAsync(newArtist);

            if (customerDbResult.IsSuccessStatusCode)
            {
                var returnUser = _mapper.Map<ArtistDto>(newArtist);

                return returnUser;
            }

            return null;
        }

        private async Task<ServiceResponse<AppUser>> CreateUserAsync(CustomerType customerType, RegistrationDto registrationDto)
        {

            if (await _userManager.Users.AnyAsync(o => o.Email == registrationDto.Email))
            {
                return new ServiceResponse<AppUser>(HttpStatusCode.BadRequest)
                {
                    ServiceResponseMessage = "That email is already in use, please try a different email address."
                };
            }

            var newUser = new AppUser
            {
                Email = registrationDto.Email,
                CustomerType = customerType,
                UserName = registrationDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, registrationDto.Password);
            await _userManager.AddToRoleAsync(newUser, customerType.ToString());

            if (!result.Succeeded)
            {
                var fnResult = new ServiceResponse<AppUser>(HttpStatusCode.BadRequest, "Something went wrong while creating your account, please try again.");
                if (result.Errors.Any())
                {
                    var errors = new Dictionary<string, string>();
                    var count = 0;
                    foreach (var error in result.Errors)
                    {
                        errors.Add((count + 1).ToString(), error.Description);
                    }
                    fnResult.Errors = errors;
                }
                return fnResult;
            }

            var user = _userManager.Users.FirstOrDefault(o => o.Email == registrationDto.Email);

            if (user == null)
            {
                return new ServiceResponse<AppUser>(HttpStatusCode.InternalServerError);
            }

            return new ServiceResponse<AppUser>(HttpStatusCode.OK)
            {
                ReturnData = user
            };
        }
    }
}
