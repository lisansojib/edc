using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Admin.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectOptionsController : ControllerBase
    {
        private readonly IEfRepository<Participant> _repository;
        private readonly IMapper _mapper;

        public SelectOptionsController(IEfRepository<Participant> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("participants")]
        public async Task<IActionResult> GetParticipants()
        {
            var records = await _repository.ListAllAsync();
            var data = _mapper.Map<List<Select2Option>>(records);
            return Ok(data);
        }
    }
}