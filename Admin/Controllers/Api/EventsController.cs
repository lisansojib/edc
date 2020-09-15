﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using Org.BouncyCastle.Math.EC.Rfc7748;
using Presentation.Admin.Interfaces;
using Presentation.Admin.Models;
using Presentation.Admin.Models.Home;

namespace Presentation.Admin.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ApiBaseController
    {
        private readonly IEventService _service;
        private readonly IEfRepository<Event> _repository;
        private readonly IEventValueFieldsService _eventValueFieldsService;
        private readonly IMapper _mapper;

        public EventsController(IEventService service
            , IEfRepository<Event> repository
            , IEventValueFieldsService eventValueFieldsService
            , IMapper mapper)
        {
            _service = service;
            _repository = repository;
            _eventValueFieldsService = eventValueFieldsService;
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

        [HttpGet("new")]
        public async Task<IActionResult> GetNew()
        {
            var data = new
            {
                SpeakersList = await _eventValueFieldsService.GetSpeakersAsync(),
                SponsorsList = await _eventValueFieldsService.GetSponsorsAsync()
            };

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.QueryableAll(x => x.Id == id).Include(x => x.Speakers).Include(x => x.Sponsors).FirstOrDefaultAsync();

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var model = _mapper.Map<EventViewModel>(entity);

            model.SpeakersList = await _eventValueFieldsService.GetSpeakersAsync();
            model.SponsorsList = await _eventValueFieldsService.GetSponsorsAsync();

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventBindingModel model)
        {
            var speakerIds = await _eventValueFieldsService.GetSpeakerIdsAsync(model.Speakers);
            var sponsorIds = await _eventValueFieldsService.GetSponsorIdsAsync(model.Sponsors);

            var entity = _mapper.Map<Event>(model);
            entity.CreatedBy = UserId;

            speakerIds.ForEach(x => entity.Speakers.Add(new Speaker { SpeakerId = x }));
            sponsorIds.ForEach(x => entity.Sponsors.Add(new Sponsor { SponsorId = x }));

            await _repository.AddAsync(entity);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EventBindingModel model)
        {
            var entity = await _repository.QueryableAll(x => x.Id == model.Id).Include(x => x.Speakers).Include(x => x.Sponsors).FirstOrDefaultAsync();

            if (entity == null) return BadRequest(new BadRequestResponseModel(ErrorTypes.BadRequest, ErrorMessages.ItemNotFound));

            var speakerIds = await _eventValueFieldsService.GetSpeakerIdsAsync(model.Speakers);
            var sponsorIds = await _eventValueFieldsService.GetSponsorIdsAsync(model.Sponsors);

            // Update Speakers
            foreach(var item in speakerIds)
            {
                if (entity.Speakers.Any(x => x.SpeakerId == item)) continue;

                entity.Speakers.Add(new Speaker { SpeakerId = item });
            }

            // Update sponsors
            foreach (var item in sponsorIds)
            {
                if (entity.Sponsors.Any(x => x.SponsorId == item)) continue;

                entity.Sponsors.Add(new Sponsor { SponsorId = item });
            }

            var deletedSpeakers = entity.Speakers.Where(x => !speakerIds.Contains(x.SpeakerId)).ToList();
            deletedSpeakers.ForEach(x => entity.Speakers.Remove(x));

            var deletedSponsors = entity.Sponsors.Where(x => !sponsorIds.Contains(x.SponsorId)).ToList();
            deletedSponsors.ForEach(x => entity.Sponsors.Remove(x));

            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.EventDate = model.EventDate;
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