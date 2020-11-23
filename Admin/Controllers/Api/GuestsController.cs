using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Admin.Models;
using Presentation.Admin.Models.Home;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ApiBaseController
    {
        private readonly IEfRepository<Guest> _repository;
        private readonly IMapper _mapper;

        public GuestsController(IEfRepository<Guest> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuestById(int id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));
            var response = _mapper.Map<GuestBindingModel>(entity);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveGuest([FromBody] GuestBindingModel model)
        {
            var entity = _mapper.Map<Guest>(model);
            
            entity.CreatedBy = UserId;
            entity.CreatedAt = DateTime.Now;

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
            entity.EmailCorp = model.EmailCorp;
            entity.EmailPersonal = model.EmailPersonal;
            entity.GuestTypeId = model.GuestTypeId;
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
            return NoContent(); 
        }
    }
}
