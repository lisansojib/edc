﻿using ApplicationCore;
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

namespace Presentation.Admin.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ZoomController : ApiBaseController
    {
        private readonly IZoomApiService _zoomApiService;
        private readonly IEfRepository<Participant> _participantRepsitory;
        private readonly IMapper _mapper;

        public ZoomController(IZoomApiService zoomApiService
            , IEfRepository<Participant> participantRepsitory
            , IMapper mapper)
        {
            _participantRepsitory = participantRepsitory;
            _zoomApiService = zoomApiService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(PagedListViewModel<ZoomUser>), 200)]
        [HttpGet("users")]
        public async Task<IActionResult> GetZoomUsers(int offset = 0, int limit = 30)
        {
            var pageNumber = offset.ToPageNumber(limit);
            var response = await _zoomApiService.GetUserListAsync(pageNumber, limit);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) return BadRequest(response.ErrorMessage);

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

            if (response.StatusCode != System.Net.HttpStatusCode.Created) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, response.ErrorMessage));

            var zoomUser = JsonConvert.DeserializeObject<ZoomUserInfo>(response.Content);

            participant.ZoomUserId = zoomUser.Id;
            await _participantRepsitory.UpdateAsync(participant);

            return Ok();
        }

        #region Zoom Meetings
        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(ZoomMeetingDTO), 200)]
        [HttpGet("meetings/{meetingId}")]
        public async Task<IActionResult> GetZoomMeetings(long meetingId)
        {
            var response = await _zoomApiService.GetMeetingAsync(meetingId);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) return BadRequest(response.ErrorMessage);

            var meeting = JsonConvert.DeserializeObject<ZoomMeetingDTO>(response.Content);

            return Ok(meeting);
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(PagedListViewModel<ZoomMeetingDTO>), 200)]
        [HttpGet("meetings")]
        public async Task<IActionResult> GetZoomMeetings(int offset = 0, int limit = 30)
        {
            var pageNumber = offset.ToPageNumber(limit);
            var response = await _zoomApiService.GetListMeetingsAsync(ZoomUserId, pageNumber, limit);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) return BadRequest(response.ErrorMessage);

            var records = JsonConvert.DeserializeObject<ListZoomMeeting>(response.Content);

            return Ok(new PagedListViewModel<ZoomMeetingDTO>(records.Meetings, (int)records.TotalRecords));
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(ZoomMeetingDTO), 200)]
        [HttpPost("meetings")]
        public async Task<IActionResult> CreateZoomMeeting(CreateingZoomMeetingDTO model)
        {
            var response = await _zoomApiService.CreateMeetingAsync(ZoomUserId, model);
            if (response.StatusCode != System.Net.HttpStatusCode.Created) return BadRequest(response.ErrorMessage);

            var zoomMeetingInfo = JsonConvert.DeserializeObject<ZoomMeetingDTO>(response.Content);
            return Ok(zoomMeetingInfo);
        }
        #endregion
    }
}
