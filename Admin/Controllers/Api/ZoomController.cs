using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Admin.Models;
using System.Threading.Tasks;
using System;
using ApplicationCore.Models;
using System.Linq;
using System.Net;

namespace Presentation.Admin.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ZoomController : ApiBaseController
    {
        private readonly IZoomApiService _zoomApiService;
        private readonly IEfRepository<Participant> _participantRepsitory;
        private readonly IEfRepository<ZoomMeeting> _zoomMeetingRepository;
        private readonly IEfRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public ZoomController(IZoomApiService zoomApiService
            , IEfRepository<Participant> participantRepsitory
            , IMapper mapper
            , IEfRepository<ZoomMeeting> zoomMeetingRepository
            , IEfRepository<Event> eventRepository)
        {
            _participantRepsitory = participantRepsitory;
            _zoomApiService = zoomApiService;
            _mapper = mapper;
            _zoomMeetingRepository = zoomMeetingRepository;
            _eventRepository = eventRepository;
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(PagedListViewModel<ZoomUser>), 200)]
        [HttpGet("users")]
        public async Task<IActionResult> GetZoomUsers(int offset = 0, int limit = 30)
        {
            var pageNumber = offset.ToPageNumber(limit);
            var response = await _zoomApiService.GetUserListAsync(pageNumber, limit);

            if (response.StatusCode != HttpStatusCode.OK) return BadRequest(response.ErrorMessage);

            var records = JsonConvert.DeserializeObject<ListZoomUser>(response.Content);

            return Ok(new PagedListViewModel<ZoomUser>(records.Users, (int)records.TotalRecords));
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(ZoomUserInfo), 200)]
        [HttpPost("users/{userId}")]
        public async Task<IActionResult> CreateZoomUser(int userId)
        {
            var participant = await _participantRepsitory.FindAsync(userId);

            var zoomUserInfo = _mapper.Map<ZoomUserInfo>(participant);
            var response = await _zoomApiService.CreateUserAsync(zoomUserInfo);

            if (response.StatusCode != HttpStatusCode.Created) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, response.ErrorMessage));

            var zoomUser = JsonConvert.DeserializeObject<ZoomUserInfo>(response.Content);

            participant.ZoomUserId = zoomUser.Id;
            await _participantRepsitory.UpdateAsync(participant);

            return Ok();
        }

        #region Zoom Meetings
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(PagedListViewModel<ZoomMeetingDTO>), 200)]
        [HttpGet("meetings")]
        public async Task<IActionResult> GetZoomMeetings(int offset = 0, int limit = 30)
        {
            var pageNumber = offset.ToPageNumber(limit);
            var response = await _zoomApiService.GetListMeetingsAsync(ZoomUserId, pageNumber, limit);

            if (response.StatusCode != HttpStatusCode.OK) return BadRequest(response.ErrorMessage);

            var record = JsonConvert.DeserializeObject<ListZoomMeeting>(response.Content);
            record.Meetings = record.Meetings.OrderBy(x => x.StartTime).ToList();

            return Ok(new PagedListViewModel<ZoomMeetingDTO>(record.Meetings, (int)record.TotalRecords));
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(ZoomMeetingDTO), 200)]
        [HttpGet("meetings/{meetingId}")]
        public async Task<IActionResult> GetZoomMeetings(long meetingId)
        {
            var response = await _zoomApiService.GetMeetingAsync(meetingId);

            if (response.StatusCode != HttpStatusCode.OK) return BadRequest(response.ErrorMessage);

            var meeting = JsonConvert.DeserializeObject<ZoomMeetingDTO>(response.Content);

            return Ok(meeting);
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(ZoomMeetingDTO), 200)]
        [HttpPost("meetings")]
        public async Task<IActionResult> CreateZoomMeeting(CreateingZoomMeetingDTO model)
        {
            model.Settings = new ZoomMeetingSettings
            {
                ParticipantVideo = false,
                //AlternativeHosts = Username
            };
            var response = await _zoomApiService.CreateMeetingAsync(ZoomUserId, model);

            if (response.StatusCode != HttpStatusCode.Created) return BadRequest(response.ErrorMessage);

            var zoomMeetingInfo = JsonConvert.DeserializeObject<ZoomMeetingDTO>(response.Content);
            var zoomMeeting = _mapper.Map<ZoomMeeting>(zoomMeetingInfo);
            zoomMeeting.CreatedBy = UserId;
            await _zoomMeetingRepository.AddAsync(zoomMeeting);

            return Ok(zoomMeeting);
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(ZoomMeetingDTO), 200)]
        [HttpDelete("meetings/{meetingId}")]
        public async Task<IActionResult> DeleteZoomMeeting(long meetingId)
        {
            var meeting = await _eventRepository.FindAsync(x => x.MeetingId == meetingId);

            if (meeting != null) return BadRequest($"Can not delete meeting. This meeting is used in an event named \"{meeting.Title}\"");

            var response = await _zoomApiService.DeleteMeetingAsync(meetingId);
            if (response.StatusCode != HttpStatusCode.NoContent) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, response.ErrorMessage));

            return Ok();
        }
        #endregion
    }
}
