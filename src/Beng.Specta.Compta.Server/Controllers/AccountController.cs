using System.Net;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Beng.Specta.Compta.Core.Dtos;
using Beng.Specta.Compta.Core.Dtos.Identities;
using Beng.Specta.Compta.Core.Interfaces;
using Beng.Specta.Compta.Infrastructure.Services;
using Beng.Specta.Compta.Server.Identities.Services;
using Beng.Specta.Compta.Server.Objects;

namespace Beng.Specta.Compta.Server.Controllers
{
    [Authorize]
    public class AccountController : BaseApiController
    {
        private static readonly UserInfoDto LoggoutUser = new();
        private readonly IAuthorizationRepository _repository;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            IAuthorizationRepository repository,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger) : base(logger)
        {
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Details()
        {
            IdentityUser identity = await _userManager.GetUserAsync(User);
            
            if (identity is null)
            {
                Logger.LogInformation($"Can't get a logged user.");
                return Ok(LoggoutUser);
            }
            
            var userInfo = identity.ToDto(User);
            Logger.LogInformation($"Get a logged user : {userInfo}");
            return Ok(userInfo);
        }

        [HttpGet("{email}"), AllowAnonymous]
        public async Task<IActionResult> Details(string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                Logger.LogWarning($"Can't get user '{email}'.");
                ModelState.AddModelError(nameof(email), $"Invalid credential.");
                return NotFound(ModelState);
            }

            Logger.LogInformation($"Find user by email: '{email}'.");
            return Ok(user.ToDto(User));
        }

        [HttpPost, AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([FromBody] SignInUserInfoDto model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                Logger.LogInformation($"User '{model.Email}' successfully log in.");
                return Ok(user.ToDto(User));
            }
            if (result.RequiresTwoFactor)
            {
                Logger.LogInformation($"Requires two factor.");
                return Ok();
            }
            if (result.IsLockedOut)
            {
                Logger.LogWarning("User account locked out.");
                ModelState.AddModelError(nameof(model.Email), "User account locked out. contact your admin.");
                return BadRequest(ModelState);
            }
            
            Logger.LogError($"Failed to log in user: '{model.Email}'");
            ModelState.AddModelError(nameof(model.Password), "Your password doesn't match");
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> SignUserOut()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Ok();
        }

        [HttpPost, AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterUserInfoDto model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            ActionResult actionResult;
            if (!result.Succeeded)
            {
                var errorDescriptions = result.Errors.Select(x => x.Description);
                Logger.LogError($"Tried to register user '{model.Email}', but failed. Errors:\n {string.Join("; ", errorDescriptions)}");

                actionResult = BadRequest(result.GetFormattedErrors());
            }
            else
            {
                Logger.LogInformation($"User '{model.Email}' created a new account with password.");
                actionResult = Ok(user);
            }

            return actionResult;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] UserInfoDto model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            ActionResult actionResult;
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                ModelState.AddModelError(nameof(model.Email), "Invalid account.");
                actionResult = BadRequest(ModelState);
            }
            else
            {
                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                // var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = _urlHelper.ResetPasswordCallbackLink(_ravenDBIdUtil.GetUrlId(user.Id), code, _httpContextAccessor.HttpContext.Request.Scheme);
                //await _emailSender.SendPasswordForgetAsync(model.Email, user.UserName, callbackUrl);
                actionResult = Ok();
            }
            
            return actionResult;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            ActionResult actionResult;
            model.Code = WebUtility.UrlDecode(model.Code);

            IdentityUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError(nameof(model.Email), "Invalid link.");
                actionResult = BadRequest(ModelState);
            }
            else
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
                if (result.Succeeded) 
                {
                    actionResult = Ok();
                }
                else
                {
                    ModelState.AddModelErrors(new Dictionary<string, List<string>>
                    {
                        [nameof(model.NewPassword)] = result.Errors.Select(e => e.Description).ToList()
                    });
                    actionResult = BadRequest(ModelState);
                }
            }
            
            return actionResult;
        }
    }
}