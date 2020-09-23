using ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface ISelectOptionService
    {
        Task<List<Select2Option>> GetCompaniesAsync();
    }
}
