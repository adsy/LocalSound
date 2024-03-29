﻿using AutoMapper.Execution;
using localsound.backend.api.Commands.Account;
using localsound.backend.api.Commands.Artist;
using localsound.backend.api.Queries.Account;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Interfaces.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Web;

namespace localsound.backend.api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<IAppUserDto>> Login(LoginSubmissionDto details)
        {
            var result = await Mediator.Send(new LoginCommand
            {
                LoginDetails = details
            });

            if (result.IsSuccessStatusCode && result.ReturnData != null)
            {
                AddCookies(result.ReturnData.AccessToken, result.ReturnData.RefreshToken);
                return Ok(result.ReturnData.UserDetails);
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<IAppUserDto>> Register(RegisterSubmissionDto details)
        {
            var result = await Mediator.Send(new RegisterCommand
            {
                RegistrationDetails = details
            });

            if (result.IsSuccessStatusCode && result.ReturnData != null)
            {
                AddCookies(result.ReturnData.AccessToken, result.ReturnData.RefreshToken);
                return Ok(result.ReturnData.UserDetails);
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [AllowAnonymous]
        [HttpPost("sign-out")]
        public new ActionResult SignOut()
        {
            AddCookies(string.Empty, string.Empty);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("get-profile-details/{profileUrl}")]
        public async Task<ActionResult<IAppUserDto>> GetUserProfileDetails(string profileUrl, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetProfileDataQuery
            {
                CurrentUser = CurrentUser.AppUserId,
                ProfileUrl = profileUrl
            }, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [AllowAnonymous]
        [HttpGet("get-profile-followers/member/{memberId}")]
        public async Task<ActionResult<IAppUserDto>> GetUserProfileFollowers([FromQuery] int page, string memberId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetProfileFollowerDataQuery
            {
                MemberId = memberId,
                Page = page,
                RetrieveFollowing = false
            }, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [AllowAnonymous]
        [HttpGet("get-profile-following/member/{memberId}")]
        public async Task<ActionResult<IAppUserDto>> GetUserProfileFollowing([FromQuery] int page, string memberId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetProfileFollowerDataQuery
            {
                MemberId = memberId,
                Page = page,
                RetrieveFollowing = true
            }, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [HttpPut]
        [Route("update-account-image/member/{memberId}/image-type/{imageType}")]
        public async Task<ActionResult> UpdateAccountImage([FromForm] FileUploadDto formData, string memberId, AccountImageTypeEnum imageType)
        {
            var updateResult = await Mediator.Send(new UpdateAccountImageCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                Photo = formData.FormFile,
                ImageType = imageType,
                FileExt = formData.FileExt
            });

            if (updateResult.IsSuccessStatusCode)
            {
                var queryResult = await Mediator.Send(new GetAccountImageQuery
                {
                    UserId = CurrentUser.AppUserId,
                    MemberId = memberId,
                    ImageType = imageType,
                });

                if (queryResult.IsSuccessStatusCode) { 
                    return Ok(queryResult.ReturnData);
                }

                return StatusCode((int)queryResult.StatusCode, "There was an error while updating your photo, please refresh your page and try again..");
            }

            return StatusCode((int)updateResult.StatusCode, updateResult.ServiceResponseMessage);
        }

        [Route("member/{memberId}/save-onboarding-data")]
        [HttpPost]
        public async Task<ActionResult> SaveOnboardingData([FromBody] SaveOnboardingDataDto data, string memberId)
        {
            var result = await Mediator.Send(new SaveProfileOnboardingDataCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                OnboardingData = data
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }
        [HttpPut]
        [Route("member/{memberId}/personal-details")]
        public async Task<ActionResult> UpdateArtistPersonalDetails([FromBody] UpdateArtistPersonalDetailsDto updateArtistDto, string memberId)
        {
            var result = await Mediator.Send(new UpdateArtistPersonalDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                UpdateArtistDto = updateArtistDto
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode);
        }

        [HttpPut]
        [Route("member/{memberId}/profile-details")]
        public async Task<ActionResult> UpdateArtistProfileDetails([FromBody] UpdateArtistProfileDetailsDto updateArtistDto, string memberId)
        {
            var result = await Mediator.Send(new UpdateArtistProfileDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                UpdateArtistDto = updateArtistDto
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode);
        }

        [HttpPost]
        [Route("follow-artist/member/{followerId}/artist/{artistId}")]
        public async Task<ActionResult> FollowArtist(string followerId, string artistId)
        {
            var result = await Mediator.Send(new UpdateArtistFollowerCommand
            {
                UserId = CurrentUser.AppUserId,
                ArtistId = artistId,
                MemberId = followerId,
                StartFollowing = true
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [HttpPost]
        [Route("unfollow-artist/member/{followerId}/artist/{artistId}")]
        public async Task<ActionResult> UnfollowArtist(string followerId, string artistId)
        {
            var result = await Mediator.Send(new UpdateArtistFollowerCommand
            {
                UserId = CurrentUser.AppUserId,
                ArtistId = artistId,
                MemberId = followerId,
                StartFollowing = false
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [HttpGet]
        public async Task<ActionResult<IAppUserDto>> CheckCurrentUserToken()
        {
            var result = await Mediator.Send(new CheckCurrentUserTokenQuery
            {
                ClaimsPrincipal = User
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode, result);
        }


        private void AddCookies(string token, string refreshToken)
        {
            // Needs to be changed to Strict when react app is moved into ASP.NET app
            Response.Cookies.Append("X-Access-Token", HttpUtility.UrlEncode(token), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddDays(7) });
            Response.Cookies.Append("X-Refresh-Token", HttpUtility.UrlEncode(refreshToken), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddDays(7) });
        }
    }
}
