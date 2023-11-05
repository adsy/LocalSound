using AutoMapper;
using AutoMapper.Execution;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;

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
        private readonly IAccountImageService _accountImageService;
        private readonly IEmailRepository _emailRepository;

        public AccountService(
            IAccountRepository accountRepository,
            ITokenRepository tokenRepository,
            IMapper mapper,
            ILogger<AccountService> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            IAccountImageService accountImageService,
            IEmailRepository emailRepository)
        {
            _accountRepository = accountRepository;
            _tokenRepository = tokenRepository;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManger = signInManger;
            _accountImageService = accountImageService;
            _emailRepository = emailRepository;
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
                var refreshToken = await _tokenRepository.CreateRefreshToken(user);

                var returnDto = null as IAppUserDto;

                if (user.CustomerType == CustomerTypeEnum.Artist && user.MemberId != null)
                {
                    var artist = await _accountRepository.GetArtistFromDbAsync(user.Id);

                    if (artist.IsSuccessStatusCode && artist.ReturnData != null)
                    {
                        returnDto = _mapper.Map<ArtistDto>(artist.ReturnData);
                        returnDto.Images = _mapper.Map<List<AccountImageDto>>(artist.ReturnData.User.Images);
                    }
                    else
                    {
                        return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                        {
                            ServiceResponseMessage = "An error occured while logging in, please try again..."
                        };
                    }
                }
                else
                {
                    var nonArtist = await _accountRepository.GetNonArtistFromDbAsync(user.Id);

                    if (nonArtist.IsSuccessStatusCode && nonArtist.ReturnData != null)
                    {
                        returnDto = _mapper.Map<NonArtistDto>(nonArtist.ReturnData);
                        returnDto.Images = _mapper.Map<List<AccountImageDto>>(nonArtist.ReturnData.User.Images);

                        foreach (var artistFollowing in nonArtist.ReturnData.User.Following)
                        {
                            returnDto.Following.Add(new ArtistSummaryDto
                            {
                                Name = artistFollowing.Artist.Name,
                                ProfileUrl = artistFollowing.Artist.ProfileUrl,
                                Images = _mapper.Map<List<AccountImageDto>>(artistFollowing.Artist.User.Images)
                            });
                        }
                    }
                    else
                    {
                        return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                        {
                            ServiceResponseMessage = "An error occured while logging in, please try again..."
                        };
                    }
                }

                if (user.MemberId != null)
                {
                    returnDto.MemberId = user.MemberId;

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.OK)
                    {
                        ReturnData = new LoginResponseDto
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            UserDetails = returnDto
                        }
                    };
                }

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(LoginAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                };
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

                var userDto = await CreateAccountAsync(registrationDetails.CustomerType, registrationDetails.RegistrationDto, userResponse.ReturnData);

                if (!userDto.IsSuccessStatusCode || userDto.ReturnData == null)
                {
                    // If we cannot add into the user database, delete the user from the aspUsers table so they can resubmit
                    await _userManager.DeleteAsync(userResponse.ReturnData);

                    var message = $"{nameof(AccountService)} - {nameof(RegisterAsync)} - User account was created but could not be stored in the customer db using email:{registrationDetails.RegistrationDto.Email}";
                    _logger.LogError(message);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError, userDto.ServiceResponseMessage ?? "Something went wrong while creating your account, please try again.");
                }

                if (userResponse.ReturnData?.MemberId != null) {

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(userResponse.ReturnData);
                    await _emailRepository.SendConfirmEmailTokenMessageAsync(token, userResponse.ReturnData.Email);

                    userDto.ReturnData.MemberId = userResponse.ReturnData.MemberId;

                    var accessToken = _tokenRepository.CreateToken(_tokenRepository.GetClaims(userResponse.ReturnData));
                    var refreshToken = await _tokenRepository.CreateRefreshToken(userResponse.ReturnData);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.OK)
                    {
                        ReturnData = new LoginResponseDto
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            UserDetails = userDto.ReturnData
                        }
                    };
                }

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(RegisterAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                };
            }
        }

        private async Task<ServiceResponse<IAppUserDto>> CreateAccountAsync(CustomerTypeEnum customerType, RegistrationDto registrationDto, AppUser user)
        {
            ServiceResponse<CustomerType>? customerDbResult;
            if (customerType == CustomerTypeEnum.Artist)
            {
                var newArtist = new Artist
                {
                    Address = registrationDto.Address,
                    Name = registrationDto.Name,
                    ProfileUrl = registrationDto.ProfileUrl,
                    PhoneNumber = registrationDto.PhoneNumber,
                    User = user,
                    AppUserId = user.Id,
                    YoutubeUrl = registrationDto.YoutubeUrl,
                    SpotifyUrl = registrationDto.SpotifyUrl,
                    SoundcloudUrl = registrationDto.SoundcloudUrl
                };

                customerDbResult = await _accountRepository.AddArtistToDbAsync(newArtist);
            }
            else
            {
                var newNonArtist = new NonArtist
                {
                    Address = registrationDto.Address,
                    FirstName = registrationDto.FirstName,
                    LastName = registrationDto.LastName,
                    PhoneNumber = registrationDto.PhoneNumber,
                    User = user,
                    AppUserId = user.Id,
                    ProfileUrl = registrationDto.ProfileUrl
                };

                customerDbResult = await _accountRepository.AddNonArtistToDbAsync(newNonArtist);
            }

            if (customerDbResult.IsSuccessStatusCode)
            {
                IAppUserDto? returnUser;

                if (customerType == CustomerTypeEnum.Artist) 
                    returnUser = _mapper.Map<ArtistDto>(customerDbResult.ReturnData);
                else
                    returnUser = _mapper.Map<NonArtistDto>(customerDbResult.ReturnData);

                return new ServiceResponse<IAppUserDto>(HttpStatusCode.OK)
                {
                    ReturnData = returnUser
                };
            }

            return new ServiceResponse<IAppUserDto>(HttpStatusCode.BadRequest)
            {
                ServiceResponseMessage = customerDbResult.ServiceResponseMessage
            };
        }

        

        private async Task<ServiceResponse<AppUser>> CreateUserAsync(CustomerTypeEnum customerType, RegistrationDto registrationDto)
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

        public async Task<ServiceResponse<IAppUserDto>> GetProfileDataAsync(string profileUrl, CancellationToken cancellationToken)
        {
            try
            {
                //Check if its an artist first
                var artistResponse = await _accountRepository.GetArtistFromDbAsync(profileUrl);
                if (artistResponse != null && artistResponse.IsSuccessStatusCode)
                {

                    var returnDto = _mapper.Map<ArtistDto>(artistResponse.ReturnData);
                    returnDto.Images = _mapper.Map<List<AccountImageDto>>(artistResponse.ReturnData?.User.Images);
                    
                    return new ServiceResponse<IAppUserDto>(HttpStatusCode.OK)
                    {
                        ReturnData = returnDto
                    };
                }

                // Check if it matches non artist if no artist matched
                var nonArtistResponse = await _accountRepository.GetNonArtistFromDbAsync(profileUrl);
                if (nonArtistResponse != null && nonArtistResponse.IsSuccessStatusCode)
                {

                    var returnDto = _mapper.Map<NonArtistDto>(artistResponse.ReturnData);
                    returnDto.Images = _mapper.Map<List<AccountImageDto>>(nonArtistResponse.ReturnData?.User.Images);

                    return new ServiceResponse<IAppUserDto>(HttpStatusCode.OK)
                    {
                        ReturnData = _mapper.Map<NonArtistDto>(nonArtistResponse.ReturnData)
                    };
                }

                // Return 404 if non artist didnt match as well
                return new ServiceResponse<IAppUserDto>(HttpStatusCode.NotFound);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(GetProfileDataAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<IAppUserDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while getting the user profile, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> UpdateAccountImage(Guid userId, string memberId, IFormFile photo, AccountImageTypeEnum imageType)
        {
            try
            {
                var accountResult = await _accountRepository.GetAppUserFromDbAsync(userId, memberId);

                if (!accountResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(accountResult.StatusCode);
                }

                var deleteResult = await _accountImageService.DeleteAccountImageIfExists(imageType, userId);

                if (deleteResult.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new ServiceResponse(deleteResult.StatusCode);
                }

                var result = await _accountImageService.UploadAccountImage(imageType, userId, photo);

                if (!result.IsSuccessStatusCode)
                {
                    return new ServiceResponse(result.StatusCode);
                }

                return result;
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(GetProfileDataAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured while saving your image, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<AccountImageDto>> GetAccountImage(Guid userId, string memberId, AccountImageTypeEnum imageType)
        {
            try
            {
                var result = await _accountRepository.GetAccountImageFromDbAsync(userId, imageType);

                if (!result.IsSuccessStatusCode || result.ReturnData == null)
                {
                    return new ServiceResponse<AccountImageDto>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<AccountImageDto>(HttpStatusCode.OK)
                {
                    ReturnData = _mapper.Map<AccountImageDto>(result.ReturnData)
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(GetAccountImage)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<AccountImageDto>(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResponse<IAppUserDto>> CheckCurrentUserToken(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

                var user = await _userManager.Users.FirstOrDefaultAsync(o => o.Email == email);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        var returnDto = null as IAppUserDto;

                        if (user.CustomerType == CustomerTypeEnum.Artist && user.MemberId != null)
                        {
                            var artist = await _accountRepository.GetArtistFromDbAsync(user.Id);

                            if (artist.IsSuccessStatusCode && artist.ReturnData != null)
                            {
                                returnDto = _mapper.Map<ArtistDto>(artist.ReturnData);
                                returnDto.Images = _mapper.Map<List<AccountImageDto>>(artist.ReturnData.User.Images);
                            }
                            else
                            {
                                return new ServiceResponse<IAppUserDto>(HttpStatusCode.InternalServerError)
                                {
                                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                                };
                            }
                        }
                        else
                        {
                            var nonArtist = await _accountRepository.GetNonArtistFromDbAsync(user.Id);

                            if (nonArtist.IsSuccessStatusCode && nonArtist.ReturnData != null)
                            {
                                returnDto = _mapper.Map<NonArtistDto>(nonArtist.ReturnData);
                                returnDto.Images = _mapper.Map<List<AccountImageDto>>(nonArtist.ReturnData.User.Images);

                                foreach(var artistFollowing in nonArtist.ReturnData.User.Following)
                                {
                                    returnDto.Following.Add(new ArtistSummaryDto
                                    {
                                        Name = artistFollowing.Artist.Name,
                                        ProfileUrl = artistFollowing.Artist.ProfileUrl,
                                        Images = _mapper.Map<List<AccountImageDto>>(artistFollowing.Artist.User.Images)
                                    });
                                }
                            }
                            else
                            {
                                return new ServiceResponse<IAppUserDto>(HttpStatusCode.InternalServerError)
                                {
                                    ServiceResponseMessage = "An error occured while logging in, please try again..."
                                };
                            }
                        }

                        if (returnDto != null)
                        {
                            returnDto.MemberId = user.MemberId;
                            return new ServiceResponse<IAppUserDto>(HttpStatusCode.OK)
                            {
                                ReturnData = returnDto
                            };
                        }
                    }

                    return new ServiceResponse<IAppUserDto>(HttpStatusCode.Unauthorized);
                }
                var message = $"{nameof(AccountService)} - {nameof(CheckCurrentUserToken)} - Login was attempted with user token which didnt match any registered email using: {email}";
                _logger.LogError(message);


                return new ServiceResponse<IAppUserDto>(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(CheckCurrentUserToken)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<IAppUserDto>(HttpStatusCode.InternalServerError);
            }
        }
    }
}
