using Microsoft.AspNetCore.Mvc;

namespace Presentation.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Events()
        {
            return View();
        }

        public IActionResult Teams()
        {
            return View();
        }

        public IActionResult Polls()
        {
            return View();
        }

        public IActionResult Announcements()
        {
            return View();
        }

        public IActionResult Companies()
        {
            return View();
        }

        public IActionResult Participants()
        {
            return View();
        }

        public IActionResult Sponsors()
        {
            return View();
        }

        public IActionResult Speakers()
        {
            return View();
        }

        public IActionResult PendingSpeakers()
        {
            return View();
        }

        public IActionResult Guests()
        {
            return View();
        }
    }
}