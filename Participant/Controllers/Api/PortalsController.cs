using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Participant.Models;

namespace Presentation.Participant.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortalsController : ApiBaseController
    {
        private readonly IEventService _eventService;
        private readonly ITeamService _teamService;
        private readonly IPollService _pollService;
        private readonly IAnnouncementService _announcementsService;

        public PortalsController(
            IEventService eventService
            , ITeamService teamService
            , IPollService pollService
            , IAnnouncementService announcementsService)
        {
            _eventService = eventService;
            _teamService = teamService;
            _pollService = pollService;
            _announcementsService = announcementsService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEvents(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _eventService.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeams(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _teamService.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpGet("polls")]
        public async Task<IActionResult> GetPolls(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _pollService.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpGet("announcements")]
        public async Task<IActionResult> GetAnnouncements(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _announcementsService.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }
    }
}
