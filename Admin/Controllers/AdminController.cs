﻿using Microsoft.AspNetCore.Mvc;

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
    }
}