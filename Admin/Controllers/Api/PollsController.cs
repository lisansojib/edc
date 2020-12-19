using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Models;
using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Admin.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ApiBaseController
    {
        private readonly IPollService _service;
        private readonly IEfRepository<Poll> _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public PollsController(IPollService service
            , IEfRepository<Poll> repository
            , IMapper mapper
            , AppDbContext dbContext)
        {
            _service = service;
            _repository = repository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _service.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel<PollDTO>(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpGet]
        [Route("new")]
        public async Task<IActionResult> GetNew()
        {
            return Ok(await _service.GetNewAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.FindAsyncWithInclude(id, x => x.DataPoints);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var model = _mapper.Map<PollDTO>(entity);

            var evtData = await _service.GetNewAsync();
            model.CohortList = evtData.CohortList;
            model.GraphTypeList = evtData.GraphTypeList;
            model.EventList = evtData.EventList;

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PollBindingModel model)
        {
            var entity = _mapper.Map<Poll>(model);
            entity.CreatedBy = UserId;

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PollBindingModel model)
        {
            var entity = await _repository.FindAsyncWithInclude(model.Id, x => x.DataPoints);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.GraphTypeId = model.GraphTypeId;
            entity.Name = model.Name;
            entity.EventId = model.EventId;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = UserId;

            entity.DataPoints = entity.DataPoints.ToList();
            
            foreach(var item in model.DataPoints)
            {
                var dataPoint = entity.DataPoints.FirstOrDefault(x => x.Id == item.Id);
                if(dataPoint == null)
                {
                    dataPoint = _mapper.Map<PollDataPoint>(item);
                    entity.DataPoints.Add(dataPoint);
                }
                else
                {
                    dataPoint.Name = item.Name;
                    dataPoint.Value = item.Value;
                    _dbContext.Entry(dataPoint).State = EntityState.Modified;
                }
            }

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