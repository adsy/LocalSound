using AutoMapper;
using Azure.Core;
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

                if (user is null)
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

                var accountResult = await _accountRepository.GetAccountFromDbAsync(user.Id);

                if (!accountResult.IsSuccessStatusCode || accountResult.ReturnData is null){
                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured while logging in, please try again..."
                    };
                }

                var accessToken = _tokenRepository.CreateToken(_tokenRepository.GetClaims(user, accountResult.ReturnData.CustomerType));
                var refreshToken = await _tokenRepository.CreateRefreshToken(user);
                var returnDto = null as IAppUserDto;


                if (accountResult.ReturnData.CustomerType == CustomerTypeEnum.Artist)
                {
                    var artist = await _accountRepository.GetArtistFromDbAsync(user.Id);

                    if (artist.IsSuccessStatusCode && artist.ReturnData != null)
                    {
                        returnDto = CreateArtistDto(artist.ReturnData);
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
                        returnDto = CreateNonArtistDto(nonArtist.ReturnData);
                    }
                    else
                    {
                        return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError)
                        {
                            ServiceResponseMessage = "An error occured while logging in, please try again..."
                        };
                    }
                }

                if (returnDto != null)
                {
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

                if (!userDto.IsSuccessStatusCode || userDto.ReturnData is null)
                {
                    // If we cannot add into the user database, delete the user from the aspUsers table so they can resubmit
                    await _userManager.DeleteAsync(userResponse.ReturnData);

                    var message = $"{nameof(AccountService)} - {nameof(RegisterAsync)} - User account was created but could not be stored in the customer db using email:{registrationDetails.RegistrationDto.Email}";
                    _logger.LogError(message);

                    return new ServiceResponse<LoginResponseDto>(HttpStatusCode.InternalServerError, userDto.ServiceResponseMessage ?? "Something went wrong while creating your account, please try again.");
                }

                if (userDto.ReturnData?.MemberId != null) {

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(userResponse.ReturnData);
                    await _emailRepository.SendConfirmEmailTokenMessageAsync(token, userResponse.ReturnData.Email);

                    userDto.ReturnData.MemberId = userDto.ReturnData.MemberId;

                    var accessToken = _tokenRepository.CreateToken(_tokenRepository.GetClaims(userResponse.ReturnData, userDto.ReturnData.CustomerType));
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
                var newArtist = new Account
                {
                    Address = registrationDto.Address,
                    Name = registrationDto.Name,
                    ProfileUrl = registrationDto.ProfileUrl,
                    PhoneNumber = registrationDto.PhoneNumber,
                    User = user,
                    AppUserId = user.Id,
                    YoutubeUrl = registrationDto.YoutubeUrl,
                    SpotifyUrl = registrationDto.SpotifyUrl,
                    SoundcloudUrl = registrationDto.SoundcloudUrl,
                    CustomerType = customerType
                };

                customerDbResult = await _accountRepository.AddArtistToDbAsync(newArtist);
            }
            else
            {
                var newNonArtist = new Account
                {
                    Address = registrationDto.Address,
                    FirstName = registrationDto.FirstName,
                    LastName = registrationDto.LastName,
                    PhoneNumber = registrationDto.PhoneNumber,
                    User = user,
                    AppUserId = user.Id,
                    ProfileUrl = registrationDto.ProfileUrl,
                    CustomerType = customerType
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

            if (user is null)
            {
                return new ServiceResponse<AppUser>(HttpStatusCode.InternalServerError);
            }

            return new ServiceResponse<AppUser>(HttpStatusCode.OK)
            {
                ReturnData = user
            };
        }

        public async Task<ServiceResponse<IAppUserDto>> GetProfileDataAsync(string profileUrl, Guid? currentUser, CancellationToken cancellationToken)
        {
            try
            {
                var returnDto = null as IAppUserDto;
                //Check if its an artist first
                var artistResponse = await _accountRepository.GetArtistFromDbAsync(profileUrl);
                if (artistResponse?.ReturnData != null && artistResponse.IsSuccessStatusCode)
                {
                    returnDto = CreateArtistDto(artistResponse.ReturnData);
                    if (artistResponse.ReturnData.Followers.Any(x => x.FollowerId == currentUser))
                    {
                        returnDto.IsFollowing = true;

                    }
                    else
                    {
                        returnDto.IsFollowing = false;
                    }

                    return new ServiceResponse<IAppUserDto>(HttpStatusCode.OK)
                    {
                        ReturnData = returnDto
                    };
                }

                // Check if it matches non artist if no artist matched
                var nonArtistResponse = await _accountRepository.GetNonArtistFromDbAsync(profileUrl);
                if (nonArtistResponse?.ReturnData != null && nonArtistResponse.IsSuccessStatusCode)
                {
                    returnDto = CreateNonArtistDto(nonArtistResponse.ReturnData);

                    return new ServiceResponse<IAppUserDto>(HttpStatusCode.OK)
                    {
                        ReturnData = returnDto
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

        public async Task<ServiceResponse<FollowerListResponseDto>> GetProfileFollowerDataAsync(string memberId, int page, bool retrieveFollowing, CancellationToken cancellationToken)
        {
            try
            {
                ServiceResponse<List<ArtistFollower>> result = null;

                if (retrieveFollowing)
                {
                    result = await _accountRepository.GetProfileFollowingFromDbAsync(memberId, page, cancellationToken);
                }
                else
                {
                    result = await _accountRepository.GetArtistFollowersFromDbAsync(memberId, page, cancellationToken);
                }

                if (result.ReturnData is null || !result.IsSuccessStatusCode)
                {
                    return new ServiceResponse<FollowerListResponseDto>(result.StatusCode);
                }

                return new ServiceResponse<FollowerListResponseDto>(HttpStatusCode.OK)
                {
                    ReturnData = new FollowerListResponseDto
                    {
                        Followers = !retrieveFollowing ? 
                        result.ReturnData.Select(x => new UserSummaryDto
                        {
                            MemberId = x.Follower.MemberId,
                            ProfileUrl = x.Follower.ProfileUrl,
                            Name = string.IsNullOrWhiteSpace(x.Follower.Name) ? $"{x.Follower.FirstName} {x.Follower.LastName}" : x.Follower.Name,
                            Images = _mapper.Map<List<AccountImageDto>>(x.Follower.Images.Where(x => !x.ToBeDeleted))
                        }).ToList() : 
                        result.ReturnData.Select(x => new UserSummaryDto
                        {
                            MemberId = x.Artist.MemberId,
                            ProfileUrl = x.Artist.ProfileUrl,
                            Name = x.Artist.Name,
                            Images = _mapper.Map<List<AccountImageDto>>(x.Artist.Images.Where(x => !x.ToBeDeleted))
                        }).ToList(),
                        CanLoadMore = result.ReturnData.Count == 30
                    }
                }; ;
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(GetProfileFollowerDataAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<FollowerListResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = !retrieveFollowing ? 
                        "An error occured while getting the profile's followers, please try again..." :
                        "An error occured while getting the profile's followings, please try again.."
                };
            }
        }

        public async Task<ServiceResponse<string>> UpdateAccountImage(Guid userId, string memberId, IFormFile photo, AccountImageTypeEnum imageType, string fileExt)
        {
            try
            {
                var accountResult = await _accountRepository.GetAccountFromDbAsync(userId, memberId);

                if (!accountResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse<string>(accountResult.StatusCode);
                }

                var deleteResult = await _accountImageService.DeleteAccountImageIfExists(imageType, userId);

                if (deleteResult.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new ServiceResponse<string>(deleteResult.StatusCode);
                }

                var result = await _accountImageService.UploadAccountImage(imageType, userId, photo, fileExt);

                if (!result.IsSuccessStatusCode)
                {
                    return new ServiceResponse<string>(result.StatusCode);
                }

                return result;
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountService)} - {nameof(GetProfileDataAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<string>(HttpStatusCode.InternalServerError)
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

                if (!result.IsSuccessStatusCode || result.ReturnData is null)
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
                        var accountResult = await _accountRepository.GetAccountFromDbAsync(user.Id);

                        if (!accountResult.IsSuccessStatusCode || accountResult.ReturnData is null)
                        {
                            return new ServiceResponse<IAppUserDto>(HttpStatusCode.InternalServerError)
                            {
                                ServiceResponseMessage = "An error occured while logging in, please try again..."
                            };
                        }

                        var returnDto = null as IAppUserDto;

                        if (accountResult.ReturnData.CustomerType == CustomerTypeEnum.Artist && accountResult.ReturnData.MemberId != null)
                        {
                            var artist = await _accountRepository.GetArtistFromDbAsync(user.Id);

                            if (artist.IsSuccessStatusCode && artist?.ReturnData != null)
                            {
                                returnDto = CreateArtistDto(artist.ReturnData);
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

                            if (nonArtist.IsSuccessStatusCode && nonArtist?.ReturnData != null)
                            {
                                returnDto = CreateNonArtistDto(nonArtist.ReturnData);
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
                            returnDto.MemberId = accountResult.ReturnData.MemberId;
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

        private IAppUserDto CreateArtistDto(Account artist)
        {
            var returnDto = _mapper.Map<ArtistDto>(artist);
            returnDto.Images = _mapper.Map<List<AccountImageDto>>(artist.Images);

            if (artist.Followers != null)
            {
                returnDto.FollowerCount = artist.Followers.Count;
            }
            else
            {
                returnDto.FollowerCount = 0;
            }

            if (artist.Following != null)
            {
                returnDto.FollowingCount = artist.Following.Count;
            }
            else
            {
                returnDto.FollowingCount = 0;
            }

            returnDto.CanAddPackage = artist.Packages.Count < 3;


            return returnDto;
        }

        private IAppUserDto CreateNonArtistDto(Account nonArtist)
        {
            var returnDto = _mapper.Map<NonArtistDto>(nonArtist);
            returnDto.Images = _mapper.Map<List<AccountImageDto>>(nonArtist.Images);

            if (nonArtist.Following != null)
            {
                returnDto.FollowingCount = nonArtist.Following.Count;
            }
            else
            {
                returnDto.FollowingCount = 0;
            }

            return returnDto;
        }
    }
}
