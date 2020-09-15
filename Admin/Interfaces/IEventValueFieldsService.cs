using ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Admin.Interfaces
{
    public interface IEventValueFieldsService
    {
        Task<List<Select2Option>> GetSpeakersAsync();
        Task<List<Select2Option>> GetSponsorsAsync();
        Task<List<int>> GetSpeakerIdsAsync(IEnumerable<Select2Option> speakers);
        Task<List<int>> GetSponsorIdsAsync(IEnumerable<Select2Option> sponsors);
    }
}
