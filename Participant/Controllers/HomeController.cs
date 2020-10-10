using System.Diagnostics;
using ApplicationCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Participant.Models;

namespace Presentation.Participant.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly PubnubKeys _pubnubKeys;

        public HomeController(IOptions<PubnubKeys> options)
        {
            _pubnubKeys = options.Value;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }        

        [HttpGet("site-settings")]
        public IActionResult GetSiteSettings()
        {
            return Ok(_pubnubKeys);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
