using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Participant.Models;

namespace Presentation.Participant.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.PARTICIPANT)]
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ApiBaseController
    {
        private readonly IEfRepository<ApplicationCore.Entities.Participant> _userRepository;
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;

        public SessionsController(IEfRepository<ApplicationCore.Entities.Participant> userRepository
            , IParticipantService participantService
            , IMapper mapper)
        {
            _userRepository = userRepository;
            _participantService = participantService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(PubnubUserViewModel), 200)]
        [HttpGet("pubnub-user/{eventId?}")]
        public async Task<IActionResult> GetPubnubUser(int eventId = 0)
        {
            var record = await _userRepository.QueryableAll(x => x.Id == UserId).Include(x => x.ParticipantTeams).FirstOrDefaultAsync();
            var user = _mapper.Map<PubnubUserViewModel>(record);

            var channelAndteamMembers = await _participantService.GetAllChannelsAsync(UserId, eventId);
            user.Channels = channelAndteamMembers.Channels;
            user.TeamMembers = channelAndteamMembers.TeamMembers;
            user.Channels.First(x => x.IsCohort).IsDefault = true;
            user.Events = channelAndteamMembers.Events;

            return Ok(user);
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(ChannelAndTeamMembersDTO), 200)]
        [HttpGet("teammembers/{teamName}")]
        public async Task<IActionResult> GetTeamMembers(string teamName)
        {
            return Ok(await _participantService.GetTeamMembersAsync(teamName));
        }
    }
}
