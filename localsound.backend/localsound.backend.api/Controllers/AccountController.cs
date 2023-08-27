using localsound.backend.api.Commands.Account;
using localsound.backend.api.Queries.Account;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Interfaces.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            if (result.IsSuccessStatusCode)
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

            if (result.IsSuccessStatusCode)
            {
                AddCookies(result.ReturnData.AccessToken, result.ReturnData.RefreshToken);
                return Ok(result.ReturnData.UserDetails);
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        [AllowAnonymous]
        [HttpPost("sign-out")]
        public async Task<ActionResult> SignOut()
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
                ProfileUrl = profileUrl
            }, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
        }

        private void AddCookies(string token, string refreshToken)
        {
            // Needs to be changed to Strict when react app is moved into ASP.NET app
            Response.Cookies.Append("X-Access-Token", HttpUtility.UrlEncode(token), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddDays(7) });
            Response.Cookies.Append("X-Refresh-Token", HttpUtility.UrlEncode(refreshToken), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddDays(7) });
        }
    }
}
