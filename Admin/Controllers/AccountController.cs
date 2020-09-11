using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Presentation.Admin.Models;
using Presentation.Interfaces;

namespace Presentation.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEfRepository<ApplicationCore.Entities.Admin> _userRepository;
        private readonly IEfRepository<ExternalLogin> _externalLoginRepository;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly Logger _logger;

        public AccountController(IEfRepository<ApplicationCore.Entities.Admin> userRepository
            , IEfRepository<ExternalLogin> externalLoginRepository
            , ITokenBuilder tokenBuilder
            , IPasswordHasher passwordHasher
            , IMapper mapper)
        {
            _userRepository = userRepository;
            _externalLoginRepository = externalLoginRepository;
            _passwordHasher = passwordHasher;
            _tokenBuilder = tokenBuilder;
            _mapper = mapper;
            _logger = LogManager.GetLogger("adminLogger");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<IActionResult> Login(string activeTab, string returnUrl)
        {
            // Clear existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ViewBag.ActiveTab = activeTab;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginBindingModel model)
        {
            var user = await _userRepository.FindAsync(x => x.Email == model.Email);

            if (user == null) return NotFound(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "User not found"));

            var (Verified, NeedsUpgrade) = _passwordHasher.Check(user.Password, model.Password);

            if (!Verified)
                return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Password does not match."));

            var response = await LoginToAppAndGetResponseAsync(user);

            return Ok(response);
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginBindingModel model)
        {
            var user = await _userRepository.FindAsync(x => x.Email == model.Email);

            if (user == null)
            {
                user = new ApplicationCore.Entities.Admin
                {
                    Email = model.Email,
                    Username = model.Email
                };

                user = await _userRepository.AddAsync(user);

                var externalLogin = _mapper.Map<ExternalLogin>(model);

                await _externalLoginRepository.AddAsync(externalLogin);

            }
            else
            {
                var externalLogin = await _externalLoginRepository.FindAsync(x => x.UserId == user.Id && x.LoginProvider == model.LoginProvider && x.ProviderKey == model.ProviderKey);
                if (externalLogin == null)
                {
                    externalLogin = _mapper.Map<ExternalLogin>(model);
                    externalLogin.UserId = user.Id;

                    await _externalLoginRepository.AddAsync(externalLogin);
                }
            }

            var response = await LoginToAppAndGetResponseAsync(user);

            return Ok(response);
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterBindingModel model)
        {
            // Check if user already exists
            var userExists = await _userRepository.ExistsAsync(x => x.Email == model.Email);
            if (userExists) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "User already exists"));

            var user = _mapper.Map<ApplicationCore.Entities.Admin>(model);

            user.Password = _passwordHasher.Hash(model.Password);
            user.Username = model.Email;

            // Later this fields will be enabled disabled by apis as required.
            user.Verified = true;
            user.Active = true;

            await _userRepository.AddAsync(user);

            _logger.Info($"{user.Username} account created.");

            var response = await LoginToAppAndGetResponseAsync(user);

            return Ok(response);
        }

        //[ProducesResponseType(typeof(ErrorResponseModel), 400)]
        //[ProducesResponseType(typeof(ErrorResponseModel), 500)]
        //[ProducesResponseType(200)]
        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordBindingModel model)
        //{
        //    var user = await _userRepository.FindAsync(x => x.Email == model.Email);
        //    if (user == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Sorry! We can't find an account with this email!"));

        //    var resetLink = $"{Request.BaseUrl()}{_uIAppSettings.ResetPasswordPath}?token={Guid.NewGuid()}&email={user.Email}";
        //    var messageBody = new StringBuilder();
        //    messageBody.Append("<br>");
        //    messageBody.Append($"Please click this <a href=\"{resetLink}\">reset password</a> link reset your password.");
        //    messageBody.Append("<br><br>Thank you!");

        //    var sendAlert = new SendAlert
        //    {
        //        Name = "Forgot Password",
        //        Email = user.Email,
        //        Type = "Forgot Password",
        //        Link = resetLink,
        //        Description = messageBody.ToString(),
        //        CreatedBy = user.Id
        //    };

        //    await _sendAlertRepository.AddAsync(sendAlert);

        //    return Ok();
        //}

        //[ProducesResponseType(typeof(ErrorResponseModel), 400)]
        //[ProducesResponseType(typeof(ErrorResponseModel), 500)]
        //[ProducesResponseType(typeof(TokenResponse), 200)]
        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordBindingModel model)
        //{
        //    var user = await _userRepository.FindAsync(x => x.Email == model.Email);
        //    if (user == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Sorry! We can't find an account with this email!"));

        //    user.Password = _passwordHasher.Hash(model.Password);
        //    user.UpdatedAt = DateTime.Now;
        //    await _userRepository.UpdateAsync(user);

        //    //Todo: Make it UTC later
        //    var expiresAtUtc = DateTime.UtcNow.AddHours(1);

        //    var token = await _tokenBuilder.BuildTokenAsync(user, expiresAtUtc);

        //    var response = new TokenResponse
        //    {
        //        AccessToken = token,
        //        ExpiresAtUtc = expiresAtUtc
        //    };

        //    return Ok(response);
        //}

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[ProducesResponseType(typeof(ErrorResponseModel), 400)]
        //[ProducesResponseType(typeof(ErrorResponseModel), 500)]
        //[ProducesResponseType(typeof(TokenResponse), 200)]
        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordBindingModel model)
        //{
        //    var user = await _userRepository.FindAsync(UserId);
        //    if (user == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Sorry! You don't have an account."));

        //    var (Verified, _) = _passwordHasher.Check(user.Password, model.CurrentPassword);
        //    if (!Verified)
        //        return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Password does not match."));

        //    user.Password = _passwordHasher.Hash(model.NewPassword);
        //    user.UpdatedAt = DateTime.Now;
        //    await _userRepository.UpdateAsync(user);

        //    //Todo: Make it UTC later
        //    var expiresAtUtc = DateTime.UtcNow.AddHours(1);

        //    var token = await _tokenBuilder.BuildTokenAsync(user, expiresAtUtc);

        //    var response = new TokenResponse
        //    {
        //        AccessToken = token,
        //        ExpiresAtUtc = expiresAtUtc
        //    };

        //    return Ok(response);
        //}

        #region Helpers
        private async Task<TokenResponse> LoginToAppAndGetResponseAsync(ApplicationCore.Entities.Admin user)
        {
            var expiresAtUtc = DateTime.UtcNow.AddHours(1);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(AdditionalClaimTypes.PhotoUrl, user.PhotoUrl)
            };

            var token = _tokenBuilder.BuildToken(user, expiresAtUtc, claims);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = expiresAtUtc
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.Info($"{user.Username} is logged in.");

            return new TokenResponse
            {
                AccessToken = token,
                ExpiresAtUtc = expiresAtUtc
            };
        }
        #endregion
    }
}