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
    public class AnnouncementsController : ApiBaseController
    {
        private readonly IAnnouncementService _service;
        private readonly IEfRepository<Announcement> _repository;
        private readonly IMapper _mapper;

        public AnnouncementsController(IAnnouncementService service
            , IEfRepository<Announcement> repository
            , IMapper mapper)
        {
            _service = service;
            _repository = repository;
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

            var record = _mapper.Map<AnnouncementDTO>(entity);

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnnouncementBindingModel model)
        {
            var entity = _mapper.Map<Announcement>(model);
            entity.CreatedBy = UserId;
            entity.ImageUrl = ""; // should this be in the form?

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AnnouncementBindingModel model)
        {
            var entity = await _repository.FindAsync(model.Id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.CallAction = model.CallAction;
            entity.LinkUrl = model.LinkUrl;
            entity.Expiration = model.Expiration;
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