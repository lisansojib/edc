using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
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
        private readonly IEfRepository<PendingSepaker> _pendingSpeakerRepository;
        private readonly IMapper _mapper;

        public SchedulesController(IEfRepository<PendingSepaker> pendingSpeakerRepository
            , IMapper mapper)
        {
            _pendingSpeakerRepository = pendingSpeakerRepository;
            _mapper = mapper;
        }

        [HttpPost("save-speaker")]
        public async Task<IActionResult> SavePendingSpeaker(PendingSepakerBindingModel model)
        {
            var entity = _mapper.Map<PendingSepaker>(model);

            if (entity.IsReferrer) entity.ReferredBy = UserId;

            entity.Username = model.Email;
            entity.CreatedBy = UserId;

            await _pendingSpeakerRepository.AddAsync(entity);

            return Ok();
        }
    }
}
