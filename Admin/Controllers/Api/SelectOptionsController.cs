using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Admin.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectOptionsController : ControllerBase
    {
        private readonly IEfRepository<Participant> _repository;
        private readonly ISelectOptionService _selectOptionService;
        private readonly IEfRepository<Event> _eventRepository;
        private readonly IEfRepository<Guest> _guestRepository;
        private readonly IMapper _mapper;

        public SelectOptionsController(IEfRepository<Participant> repository
            , ISelectOptionService selectOptionService
            , IEfRepository<Event> eventRepository
            , IMapper mapper
            , IEfRepository<Guest> guestRepository)
        {
            _repository = repository;
            _selectOptionService = selectOptionService;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _guestRepository = guestRepository;
        }

        [HttpGet("participants")]
        public async Task<IActionResult> GetParticipants()
        {
            var records = await _repository.ListAllAsync();
            var data = _mapper.Map<List<Select2Option>>(records);
            return Ok(data);
        }

        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies()
        {
            return Ok(await _selectOptionService.GetCompaniesAsync());
        }

        [HttpGet("related-events")]
        public async Task<IActionResult> GetRelatedEvents(DateTime date)
        {
            var records = await _eventRepository.ListAllAsync(x => x.EventDate.Date == date.Date);
            var response = records.Select(x => new Select2Option { Id = x.SessionId, Text = x.Title });
            return Ok(response);
        }

        [HttpGet("guest")]
        public IActionResult GetGuests()
        {
            var guests = _guestRepository.QueryableAll().Select(x => new Select2Option { Id = x.Id.ToString(), Text = x.Email, Desc = x.FirstName + " " + x.LastName }).ToList();
            return Ok(guests);
        }
    }
}