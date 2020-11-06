using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Participant.Controllers
{
    [Authorize]
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
        public IActionResult Members()
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
