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
        public ActionResult Session()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Members()
        {
            return View();
        }
    }
}
