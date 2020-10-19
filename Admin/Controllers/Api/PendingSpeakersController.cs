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
using Microsoft.EntityFrameworkCore;
using Presentation.Admin.Models;

namespace Presentation.Admin.Controllers.Api
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PendingSpeakersController : ApiBaseController
    {
        private readonly IEfRepository<PendingSpeaker> _repository;
        private readonly IPendingSpeakerService _service;
        public PendingSpeakersController(IEfRepository<PendingSpeaker> repository
            , IPendingSpeakerService service
            )
        {
            _repository = repository;
            _service = service;

        }

        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 10, string filter = null, string sort = null, string order = null)
        {
            var orderBy = string.IsNullOrEmpty(sort) ? "" : $"ORDER BY {sort} {order}";
            var records = await _service.GetPagedAsync(offset, limit, filter, orderBy);

            var response = new PagedListViewModel(records, records.FirstOrDefault()?.Total);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStatus(PendingSpeakerBindingModel model)
        {
            var entity = await _repository.FindAsync(model.Id);

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            if (model.IsAccepted)
            {
                entity.IsAccepted = true;
                entity.AcceptDate = DateTime.Now;
                entity.AcceptedBy = UserId;
            }
            else
            {
                entity.IsRejected = true;
                entity.RejectDate = DateTime.Now;
                entity.RejectedBy = UserId;
            }

            await _repository.UpdateAsync(entity);

            return NoContent();
        }
    }
}
