using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Participant.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestSessionController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public GuestSessionController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpGet("event-info/{eventId}")]
        public async Task<IActionResult> GetEventInfo(int eventId)
        {
            var records = await _participantService.GetEventDetailsAsync(eventId);
            return Ok(records);
        }
    }
}
