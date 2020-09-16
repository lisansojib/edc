using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Admin.Models;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ApiBaseController
    {
        private readonly ITeamService _service;
        private readonly IEfRepository<Team> _repository;
        private readonly IEfRepository<Participant> _participentRepositoy;
        private readonly IMapper _mapper;

        public TeamsController(ITeamService service
            , IEfRepository<Team> repository
            , IEfRepository<Participant> participentRepositoy
            , IMapper mapper)
        {
            _service = service;
            _repository = repository;
            _participentRepositoy = participentRepositoy;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.QueryableAll(x => x.Id == id).Include(x => x.ParticipantTeams).FirstOrDefaultAsync();

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var model = _mapper.Map<TeamViewModel>(entity);

            var participants = await _participentRepositoy.ListAllAsync();
            model.ParticipantsList = _mapper.Map<List<Select2Option>>(participants);

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamBindingModel model)
        {
            var entity = _mapper.Map<Team>(model);
            entity.CreatedBy = UserId;

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TeamBindingModel model)
        {
            var entity = await _repository.QueryableAll(x => x.Id == model.Id).Include(x => x.ParticipantTeams).FirstOrDefaultAsync();

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));
                        
            // Update Participants
            foreach(var item in model.Participants)
            {
                if (entity.ParticipantTeams.Any(x => x.TeamMemberId == item)) continue;

                entity.ParticipantTeams.Add(new ParticipantTeam { TeamId = entity.Id, TeamMemberId = item });
            }

            var deletedParticipantTeams = entity.ParticipantTeams.Where(x => !model.Participants.Contains(x.TeamMemberId)).ToList();
            deletedParticipantTeams.ForEach(x => entity.ParticipantTeams.Remove(x));

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = UserId;

            await _repository.UpdateAsync(entity);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            await _repository.DeleteAsync(entity);

            return Ok();
        }
    }
}