using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore;
using MelaMandiUI.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Admin.Models;

namespace Presentation.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly IDeSerializeJwtToken _deSerializeJwtToken;

        public AccountController(IDeSerializeJwtToken deSerializeJwtToken)
        {
            _deSerializeJwtToken = deSerializeJwtToken;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string activeTab, string returnUrl)
        {
            // Clear existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ViewBag.ActiveTab = activeTab;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]TokenBindingModel model)
        {
            var claims = _deSerializeJwtToken.GetClaims(model.AccessToken);
            if (claims.GetUserId() <= 0) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Can not authenticate user."));

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = claims.GetIsPersistent(),
                ExpiresUtc = model.ExpiresAtUtc
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpGet]
        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RecoverySuccessful()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordBindingModel { Token = token, Email = email };
            return View(model);
        }
    }
}