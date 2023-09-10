using localsound.backend.api.Commands.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace localsound.backend.api.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        public TokenController()
        {
            
        }

        [AllowAnonymous]
        [Route("refresh-token")]
        [HttpPost]
        public async Task<ActionResult> ValidateRefreshToken()
        {
            var refreshToken = HttpUtility.UrlDecode(HttpContext.Request.Cookies["X-Refresh-Token"]);
            var accessToken = HttpUtility.UrlDecode(HttpContext.Request.Cookies["X-Access-Token"]);

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest();
            }

            var result = await Mediator.Send(new ValidateRefreshTokenCommand()
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken
            });

            if (result.IsSuccessStatusCode && result.ReturnData != null)
            {
                AddCookies(result.ReturnData.AccessToken, result.ReturnData.RefreshToken);
                return Ok();
            }

            return StatusCode((int)result.StatusCode);
        }

        //[Authorize]
        //[HttpPost("confirm-email")]
        //public async Task<ActionResult<IAppUserDto>> ConfirmEmail([FromBody] ConfirmEmailDto emailToken)
        //{
        //    var result = await Mediator.Send(new ConfirmEmailTokenCommand
        //    {
        //        EmailToken = emailToken.Token,
        //        User = User
        //    });

        //    if (result.IsSuccessStatusCode)
        //    {
        //        AddCookies(result.ReturnData.AccessToken, result.ReturnData.RefreshToken);
        //        return Ok(result.ReturnData.UserDetails);
        //    }

        //    return StatusCode((int)result.StatusCode, result);
        //}

        //[Authorize]
        //[HttpPost("resend-email-token")]
        //public async Task<ActionResult> ResendConfirmEmailToken()
        //{
        //    var result = await Mediator.Send(new ResendConfirmEmailTokenCommand
        //    {
        //        User = User
        //    });

        //    if (result.IsSuccessStatusCode)
        //    {
        //        return Ok();
        //    }

        //    return StatusCode((int)result.StatusCode, result);
        //}


        private void AddCookies(string token, string refreshToken)
        {
            // Needs to be changed to Strict when react app is moved into ASP.NET app
            Response.Cookies.Append("X-Access-Token", HttpUtility.UrlEncode(token), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddDays(7) });
            Response.Cookies.Append("X-Refresh-Token", HttpUtility.UrlEncode(refreshToken), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddDays(7) });
        }
    }
}
