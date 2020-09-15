using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Upcoming()
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
    }
}