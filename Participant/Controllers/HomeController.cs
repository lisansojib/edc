using System;
using System.Diagnostics;
using System.Security.Cryptography;
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

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

        [HttpGet("zoom-signature")]
        public IActionResult GetZoomSignature(int meetingNo, int roles)
        {
            var ts = (DateTime.UtcNow.ToUniversalTime().ToTimestamp() - 30000).ToString();
            string message = $"{ZoomSettings.API_KEY}{meetingNo}{ts}{roles}";

            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(ZoomSettings.API_SECRET);
            byte[] messageBytesTest = encoding.GetBytes(message);
            string msgHashPreHmac = System.Convert.ToBase64String(messageBytesTest);
            byte[] messageBytes = encoding.GetBytes(msgHashPreHmac);

            var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            string msgHash = Convert.ToBase64String(hashmessage);
            string tToken = $"{ZoomSettings.API_KEY}.{meetingNo}.{ts}.{roles}.{msgHash}";
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes(tToken);
            var token = Convert.ToBase64String(tokenBytes).TrimEnd(new char[]{ '=' });
            return Ok(token);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
