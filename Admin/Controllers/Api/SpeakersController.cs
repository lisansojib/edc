using System;
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
using Presentation.Admin.Models;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ApiBaseController
    {
        private readonly ISpeakerService _service;
        private readonly IEfRepository<Speaker> _repository;
        private readonly ISelectOptionService _selectOptionService;
        private readonly IMapper _mapper;

        public SpeakersController(ISpeakerService service
            , IEfRepository<Speaker> repository
            , ISelectOptionService selectOptionService
            , IMapper mapper)
        {
            _service = service;
            _repository = repository;
            _selectOptionService = selectOptionService;
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
            var entity = await _repository.FindAsync(id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var record = _mapper.Map<SpeakerDTO>(entity);
            record.CompanyList = await _selectOptionService.GetCompaniesAsync();

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpeakerBindingModel model)
        {
            var entity = _mapper.Map<Speaker>(model);
            entity.CreatedBy = UserId;

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SpeakerBindingModel model)
        {
            var entity = await _repository.FindAsync(model.Id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Title = model.Title;
            entity.CompanyId = model.CompanyId;
            entity.UpdatedBy = UserId;
            entity.UpdatedAt = DateTime.Now;

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