using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Participant.Controllers
{
    public class GuestController : Controller
    {
        private readonly IEfRepository<Event> _eventRepository;

        public GuestController(IEfRepository<Event> eventRepository)
        {
            _eventRepository = eventRepository;
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
            var record = await _eventRepository.FindAsync(eventId);
            ViewBag.MeetingId = record.MeetingId;
            ViewBag.MeetingPassword = record.MeetingPassword;
            ViewBag.IsGuest = 1;

            return View();
        }

        [HttpGet]
        public IActionResult Meeting()
        {
            return View();
        }
    }
}
