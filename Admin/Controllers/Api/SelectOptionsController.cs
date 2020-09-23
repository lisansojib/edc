using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public SelectOptionsController(IEfRepository<Participant> repository
            , ISelectOptionService selectOptionService
            , IMapper mapper)
        {
            _repository = repository;
            _selectOptionService = selectOptionService;
            _mapper = mapper;
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
    }
}