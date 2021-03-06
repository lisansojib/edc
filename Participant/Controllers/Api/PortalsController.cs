﻿using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Participant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Participant.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.PARTICIPANT)]
    [Route("api/[controller]")]
    [ApiController]
    public class PortalsController : ApiBaseController
    {
        private readonly IEventService _eventService;
        private readonly ITeamService _teamService;
        private readonly IPollService _pollService;
        private readonly IAnnouncementService _announcementsService;
        private readonly IEfRepository<ApplicationCore.Entities.Participant> _participantRepository;
        private readonly IEfRepository<PollDataPoint> _pollDataPointRepository;
        private readonly IMapper _mapper;

        public PortalsController(
            IEventService eventService
            , ITeamService teamService
            , IPollService pollService
            , IAnnouncementService announcementsService
            , IEfRepository<ApplicationCore.Entities.Participant> participantRepository
            , IEfRepository<PollDataPoint> pollDataPointRepository
            , IMapper mapper)
        {
            _eventService = eventService;
            _teamService = teamService;
            _pollService = pollService;
            _announcementsService = announcementsService;
            _participantRepository = participantRepository;
            _pollDataPointRepository = pollDataPointRepository;
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

        [HttpGet("my-team-members")]
        public async Task<IActionResult> GetMyTeamMembers()
        {
            var records = await _teamService.GetAllTeamMembersAsync(UserId);
            records.ForEach(x => x.PhotoUrl = x.PhotoUrl.ToThumbnailImagePath());
            return Ok(records);
        }

        [HttpGet("participant/{id}")]
        public async Task<IActionResult> GetParticipantDetails(int id)
        {
            var participant = await _participantRepository.FindAsync(id);
            if (participant == null) return BadRequest(new BadRequestResponseModel(ErrorMessages.ItemNotFound, "Participant not found"));
            
            var response = _mapper.Map<ParticipantDTO>(participant);
            return Ok(response);
        }

        [HttpGet("poll-datapoints/{portalId}")]
        public async Task<IActionResult> GetPollDataPoints(int portalId)
        {
            var records = await _pollDataPointRepository.ListAllAsync(x => x.PollId == portalId);
            var dataPoints = _mapper.Map<List<PollDataPointDTO>>(records);
            return Ok(dataPoints);
        }
    }
}
