using localsound.backend.api.Commands.Account;
using localsound.backend.Domain.Model.Dto;
using localsound.backend.Domain.Model.Dto.Entity;
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
        [HttpPost("customer-login")]
        public async Task<ActionResult<NonArtistDto>> UserLogin(LoginDataDto details)
        {
            var result = await Mediator.Send(new LoginCommand
            {
                UserDetails = details
            });

            if (result.IsSuccessStatusCode)
            {
                AddCookies(result.ReturnData.AccessToken, result.ReturnData.RefreshToken);
                return Ok(result.ReturnData.UserDetails);
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
