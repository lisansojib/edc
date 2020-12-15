using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services.Home;
using ApplicationCore.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Admin.Models;
using Presentation.Admin.Models.Home;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ApiBaseController
    {
        private readonly IEfRepository<Guest> _repository;
        private readonly IGuestService _service;
        private readonly IMapper _mapper;

        public GuestsController(IEfRepository<Guest> repository
            , IGuestService service
            , IMapper mapper)
        {
            _repository = repository;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _service.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel<GuestDTO>(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));
            var response = _mapper.Map<GuestBindingModel>(entity);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] GuestBindingModel model)
        {
            var entity = _mapper.Map<Guest>(model);            
            entity.CreatedBy = UserId;
            entity.Email = model.EmailPersonal;

            await _repository.AddAsync(entity);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGuest([FromBody] GuestBindingModel model)
        {
            var entity = await _repository.FindAsync(model.Id);
            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));
            
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.LinkedinUrl = model.LinkedinUrl;
            entity.PhoneCorp = model.PhoneCorp;
            entity.PhonePersonal = model.PhonePersonal;
            entity.EmailCorp = model.EmailCorp;
            entity.EmailPersonal = model.EmailPersonal;
            entity.Email = model.EmailPersonal;
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedBy = UserId;

            await _repository.UpdateAsync(entity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));
            await _repository.DeleteAsync(entity);
            return Ok(); 
        }

        [HttpPut("convert-to-member/{id}")]
        public async Task<IActionResult> ConvertToMember(int id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            entity.Role = GuestRoles.MEMBER;

            await _repository.UpdateAsync(entity);

            return Ok();
        }
    }
}
