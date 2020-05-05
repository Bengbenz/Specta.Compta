using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Beng.Specta.Compta.Core.DTOs;
using Beng.Specta.Compta.SharedKernel.Interfaces;

namespace Beng.Specta.Compta.Server.Api
{
    public class UserController : BaseApiController
    {
        private static UserInfoDTO LoggedOutUser = new UserInfoDTO { IsAuthenticated = false };

        private readonly IRepository _repository;

        public UserController(
            IRepository repository,
            ILogger<UserController> logger) : base(logger)
        {
            _repository = repository;
        }

        [HttpGet("/user")]
        public UserInfoDTO GetUserInfo()
        {   
            UserInfoDTO userInfo = LoggedOutUser;
            if (User.Identity.IsAuthenticated)
            {
                userInfo = new UserInfoDTO(User.Identity.Name, true);
            }
            return userInfo;
        }

        [HttpGet("/user/signin")]
        public async Task SignIn(string redirectUri)
        {
            if (string.IsNullOrEmpty(redirectUri) || !Url.IsLocalUrl(redirectUri))
            {
                redirectUri = "/";
            }

            await HttpContext.ChallengeAsync(new AuthenticationProperties { RedirectUri = redirectUri });
        }

        [HttpGet("/user/signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            // await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }
    }
}