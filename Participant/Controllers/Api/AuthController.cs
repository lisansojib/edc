using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using Presentation.Participant.Controllers.Api;
using Presentation.Participant.Interfaces;
using Presentation.Participant.Models;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Participant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiBaseController
    {
        private readonly IEfRepository<ApplicationCore.Entities.Participant> _userRepository;
        private readonly IEfRepository<ExternalLogin> _externalLoginRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IZoomApiService _zoomApiService;
        private readonly IMapper _mapper;
        private readonly Logger _logger;

        public AuthController(IEfRepository<ApplicationCore.Entities.Participant> userRepository
            , IEfRepository<ExternalLogin> externalLoginRepository
            , IEmailService emailService
            , ITokenBuilder tokenBuilder
            , IPasswordHasher passwordHasher
            , IZoomApiService zoomApiService
            , IMapper mapper)
        {
            _userRepository = userRepository;
            _externalLoginRepository = externalLoginRepository;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _tokenBuilder = tokenBuilder;
            _zoomApiService = zoomApiService;
            _mapper = mapper;
            _logger = LogManager.GetLogger("participantLogger");
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpPost("token")]
        public async Task<IActionResult> Login([FromBody]LoginBindingModel model)
        {
            var user = await GetLoginUserAsync(model.Email);

            if (user == null ) return NotFound(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "User not found"));

            var (Verified, NeedsUpgrade) = _passwordHasher.Check(user.Password, model.Password);

            if (!Verified) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Password does not match."));

            if (!user.Active || !user.Verified) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "You are not active user."));

            return Ok(GetTokenResponse(user, model.RememberMe));
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
                user = new ApplicationCore.Entities.Participant
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

            if (!user.Active || !user.Verified) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "You are not active user."));

            var loginUser = _mapper.Map<ApplicationCore.Entities.Participant, UserViewModel>(user);

            return Ok(GetTokenResponse(loginUser, false));
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

            var user = _mapper.Map<ApplicationCore.Entities.Participant>(model);

            user.Password = _passwordHasher.Hash(model.Password);
            user.Username = model.Email;

            // Later this fields will be enabled disabled by apis as required.
            user.Verified = false;
            user.Active = false;

            await _userRepository.AddAsync(user);

            _logger.Info($"{user.Username} account created.");

            // Create zoom account
            var zoomUserInfo = _mapper.Map<ZoomUserInfo>(user);
            var response = await _zoomApiService.CreateUserAsync(zoomUserInfo);
            if(response.StatusCode != System.Net.HttpStatusCode.OK) _logger.Error(response.ErrorMessage);

            var zoomUser = JsonConvert.DeserializeObject<ZoomUserInfo>(response.Content);

            user.ZoomUserId = zoomUser.Id;
            await _userRepository.UpdateAsync(user);

            return Ok();
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(200)]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordBindingModel model)
        {
            var user = await _userRepository.FindAsync(x => x.Email == model.Email);
            if (user == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Sorry! We can't find an account with this email!"));

            var resetLink = $"{Url.Action(action: "Reset", "")}?token={Guid.NewGuid()}&email={user.Email}";
            var messageBody = new StringBuilder();
            messageBody.Append("<br>");
            messageBody.Append($"Please click this <a href=\"{resetLink}\">reset password</a> link reset your password.");
            messageBody.Append("<br><br>Thank you!");

            await _emailService.SendEmailAsync(user.Username, user.Email, "Retrieve your password.", messageBody.ToString());

            _logger.Info($"Password reset link is sent to {user.Username}");

            return Ok();
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordBindingModel model)
        {
            var user = await _userRepository.FindAsync(x => x.Email == model.Email);
            if (user == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Sorry! We can't find an account with this email!"));

            user.Password = _passwordHasher.Hash(model.Password);
            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            _logger.Info($"{user.Username} reseted password.");

            var loginUser = _mapper.Map<ApplicationCore.Entities.Participant, UserViewModel>(user);

            return Ok(GetTokenResponse(loginUser, false));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordBindingModel model)
        {
            var user = await _userRepository.FindAsync(User.Claims.UserId());
            if (user == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Sorry! You don't have an account."));

            var (Verified, _) = _passwordHasher.Check(user.Password, model.CurrentPassword);
            if (!Verified)
                return BadRequest(new BadRequestResponseModel(ErrorMessages.AuthenticatinError, "Password does not match."));

            user.Password = _passwordHasher.Hash(model.NewPassword);
            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            _logger.Info($"{user.Username} changed password.");

            var loginUser = _mapper.Map<ApplicationCore.Entities.Participant, UserViewModel>(user);

            return Ok(GetTokenResponse(loginUser, false));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpGet("me")]
        public async Task<IActionResult> GetAdmin()
        {
            var entity = await _userRepository.FindAsync(User.Claims.UserId());

            if (entity == null)
            {
                return NotFound();
            }

            var user = _mapper.Map<ParticipantViewModel>(entity);
            return Ok(user);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(200)]
        [HttpPost("create-zoom")]
        public async Task<IActionResult> CreateZoomUser()
        {
            var user = await _userRepository.FindAsync(UserId);
            var zoomUserInfo = _mapper.Map<ZoomUserInfo>(user);
            var response = await _zoomApiService.CreateUserAsync(zoomUserInfo);

            if (response.StatusCode != System.Net.HttpStatusCode.Created) return BadRequest(response.ErrorMessage);

            var zoomUser = JsonConvert.DeserializeObject<ZoomUserInfo>(response.Content);

            user.ZoomUserId = zoomUser.Id;
            await _userRepository.UpdateAsync(user);

            return Ok();
        }

        #region Helpers
        private async Task<UserViewModel> GetLoginUserAsync(string email)
        {
            UserViewModel user = _mapper.Map<ApplicationCore.Entities.Participant, UserViewModel>(await _userRepository.FindAsync(x => x.Email == email));

            //if (user == null)
            //{
            //    var guest = await _guestRepository.FindAsync(x => x.Email == email);
            //    user = _mapper.Map<Guest, UserViewModel>(guest);
            //    user.IsGuest = true;
            //    user.Role = guest.Role;
            //}

            return user;
        }

        private TokenResponse GetTokenResponse(UserViewModel user, bool isPersistent)
        {
            var expiresAtUtc = DateTime.UtcNow.AddDays(7);

            var token = _tokenBuilder.BuildToken(user, expiresAtUtc, isPersistent);

            _logger.Info($"{user.Username} requested new token.");

            return new TokenResponse
            {
                AccessToken = token,
                ExpiresAtUtc = expiresAtUtc
            };
        }
        #endregion
    }
}