using System.Threading.Tasks;
using ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectOptionsController : ControllerBase
    {
        private readonly ISelectOptionService _selectOptionService;

        public SelectOptionsController(ISelectOptionService selectOptionService)
        {
            _selectOptionService = selectOptionService;
        }

        [ProducesResponseType(typeof(ErrorResponseModel), 400)]
        [ProducesResponseType(typeof(ErrorResponseModel), 500)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies()
        {
            return Ok(await _selectOptionService.GetCompaniesAsync());
        }
    }
}