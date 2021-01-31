using Library.Core.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Securites;
using Services;
using System;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserSellerService _userService;
        private readonly IOnlineService _onlineService;
        private readonly IMobileVersionService _mobileVersionService;

        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(IUserSellerService userService
            , IJwtAuthManager jwtAuthManager
            , IOnlineService onlineService
            , IMobileVersionService mobileVersionService)
        {
            _userService = userService;
            _onlineService = onlineService;
            _mobileVersionService = mobileVersionService;
            _jwtAuthManager = jwtAuthManager;
        }


        [HttpGet("checkversion/{versionName}"), AllowAnonymous]
        public ActionResult CheckVersion(string versionName)
        {
            return Ok(_mobileVersionService.IsMobileVersionLatest(versionName));
        }

        [HttpPost("login/{deviceCode}/{password}"), AllowAnonymous]
        public ActionResult Login(string deviceCode, string password)
        {
            if (!_onlineService.IsSeverOnline()) return Ok(false);

            if (!_userService.IsValidUserCredentials(deviceCode, password))
                return Unauthorized();

            var user = _userService.GetAsync(t => t.device_code == deviceCode && t.us_pwd == password && t.us_status == 1).Result;

            var role = _userService.GetUserRole();
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,deviceCode.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(deviceCode.ToString(), claims);

            return Ok(new
            {
                DeviceCode = deviceCode,
                Role = role,
                jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            return Ok();
        }



        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
