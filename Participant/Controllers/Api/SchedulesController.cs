using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services.Portal;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Participant.Models;
using System.Threading.Tasks;

namespace Presentation.Participant.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ApiBaseController
    {
        private readonly IEfRepository<PendingSpeaker> _pendingSpeakerRepository;
        private readonly IMapper _mapper;
        private readonly IScheduleService _service;

        public SchedulesController(IEfRepository<PendingSpeaker> pendingSpeakerRepository
            , IMapper mapper
            , IScheduleService scheduleService)
        {
            _pendingSpeakerRepository = pendingSpeakerRepository;
            _mapper = mapper;
            _service = scheduleService;
        }

        [HttpPost("save-speaker")]
        public async Task<IActionResult> SavePendingSpeaker(PendingSpeakerBindingModel model)
        {
            var exists = await _pendingSpeakerRepository.ExistsAsync(x => x.Email == model.Email && x.PanelId == model.PanelId);
            if (exists) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, "Already exists!"));

            var entity = _mapper.Map<PendingSpeaker>(model);

            if (entity.IsReferrer) entity.ReferredBy = UserId;

            entity.Username = model.Email;
            entity.CreatedBy = UserId;

            await _pendingSpeakerRepository.AddAsync(entity);

            return Ok();
        }

        [HttpGet("new-speaker")]
        public async Task<IActionResult> GetNewSpeaker()
        {
            return Ok(await _service.GetNewPendingSpeakerAsync(UserId));
        }
    }
}
