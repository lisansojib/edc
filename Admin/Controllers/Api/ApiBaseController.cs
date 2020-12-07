using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Admin.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        protected int UserId => User.Claims.GetUserId();
        protected string Username => User.Claims.GetUsername();
        protected string UserRole => User.Claims.GetUserRole();
        protected string BaseUrl => Request.Scheme + "://" + Request.Host + Request.PathBase;
        protected string ZoomUserId => User.Claims.GetZoomUserId();
    }
}