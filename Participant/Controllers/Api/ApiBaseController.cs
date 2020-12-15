using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Participant.Controllers.Api
{
    public abstract class ApiBaseController : ControllerBase
    {
        protected int UserId => User.Claims.UserId();
        protected string Username => User.Claims.Username();
        protected string UserRole => User.Claims.UserRole();
        protected string BaseUrl => Request.Scheme + "://" + Request.Host + Request.PathBase;
    }
}