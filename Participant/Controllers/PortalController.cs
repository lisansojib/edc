using ApplicationCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Participant.Controllers
{
    [Authorize(Roles = UserRoles.PARTICIPANT)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PortalController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Session(int eventId = 0)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        [HttpGet]
        public IActionResult Zoom()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TeamMembers()
        {
            ViewBag.UUId = User.Claims.UUId();
            return View();
        }

        [HttpGet]
        public IActionResult MyTeams()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Schedule()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Meeting()
        {
            return View();
        }
    }
}
