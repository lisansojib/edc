using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace Presentation.Participant.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = UserRoles.GUEST)]
    public class GuestController : Controller
    {
        private readonly IEfRepository<Event> _eventRepository;
        private readonly Logger _logger;

        public GuestController(IEfRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
            _logger = LogManager.GetLogger("participantLogger");
        }

        [HttpGet]
        public IActionResult Session(int eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Zoom(int eventId)
        {
            try
            {
                var record = await _eventRepository.FindAsync(eventId);
                ViewBag.MeetingId = record.MeetingId;
                ViewBag.MeetingPassword = record.MeetingPassword;
                ViewBag.IsGuest = 1;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Meeting()
        {
            return View();
        }
    }
}
