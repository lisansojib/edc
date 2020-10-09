using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Participant.Models;
using System;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ApplicationCore.DTOs;
using ApplicationCore;

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
        private readonly IEfRepository<ApplicationCore.Entities.Participant> _participantRepository;
        private readonly IMapper _mapper;

        public PortalsController(
            IEventService eventService
            , ITeamService teamService
            , IPollService pollService
            , IAnnouncementService announcementsService
            , IEfRepository<ApplicationCore.Entities.Participant> participantRepository
            , IMapper mapper)
        {
            _eventService = eventService;
            _teamService = teamService;
            _pollService = pollService;
            _announcementsService = announcementsService;
            _participantRepository = participantRepository;
            _mapper = mapper;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("my-teams")]
        public async Task<IActionResult> GetMyTeams()
        {
            var teams = await _teamService.GetMyTeamsAsync(UserId);

            var response = teams.GroupBy(
                p => new { Id = p.TeamId, Name = p.TeamName },
                pt => new { Id = pt.ParticipantTeamId, pt.ParticipantName, pt.TeamMemberId, PhotoUrl = pt.PhotoUrl.ToThumbnailImagePath() },
                (p, pt) => new TeamViewModel { 
                    Id = p.Id, 
                    Name = p.Name, 
                    Participants = pt.Select(x => new TeamParticipantViewModel { 
                        Id = x.Id, 
                        TeamMemberId = x.TeamMemberId, 
                        ParticipantName = x.ParticipantName, 
                        PhotoUrl = x.PhotoUrl,
                        Disable = x.TeamMemberId == UserId
                    }).ToList() });

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("participant/{id}")]
        public async Task<IActionResult> GetParticipantDetails(int id)
        {
            var participant = await _participantRepository.QueryableAll(x => x.Id == id).Include(x => x.Company).FirstOrDefaultAsync();
            if (participant == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.ItemNotFound, "Participant not found"));
            
            var response = _mapper.Map<ParticipantDTO>(participant);
            return Ok(response);
        }
    }
}
