using ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface ISelectOptionService
    {
        Task<IEnumerable<Select2Option>> GetCompaniesAsync();
    }
}
