using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Admin.Models;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PendingSpeakersController : ApiBaseController
    {
        private readonly IEfRepository<PendingSpeaker> _repository;
        private readonly IEfRepository<Speaker> _speakerRepository;
        private readonly IPendingSpeakerService _service;
        private readonly IMapper _mapper;
        public PendingSpeakersController(IEfRepository<PendingSpeaker> repository
            , IEfRepository<Speaker> speakerRepository
            , IPendingSpeakerService service
            , IMapper mapper)
        {
            _repository = repository;
            _speakerRepository = speakerRepository;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _service.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpPut("create-speaker/{id}")]
        public async Task<IActionResult> CreateSpeaker(int id)
        {
            var entity = await _repository.FindAsync(id);

            var pendingSpeaker = _mapper.Map<Speaker>(entity);
            entity.CreatedBy = UserId;

            await _speakerRepository.AddAsync(pendingSpeaker);

            entity = await _repository.FindAsync(id);
            entity.IsAccepted = true;
            entity.AcceptDate = DateTime.Now;
            entity.AcceptedBy = UserId;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = UserId;

            await _repository.UpdateAsync(entity);

            return Ok();
        }

        /// <summary>
        /// Rejects Pending Speaker
        /// </summary>
        /// <param name="id"></param>
        /// <returns>NoContent</returns>
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.IsRejected = true;
            entity.RejectDate = DateTime.Now;
            entity.RejectedBy = UserId;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = UserId;

            await _repository.UpdateAsync(entity);

            return Ok();
        }
    }
}
