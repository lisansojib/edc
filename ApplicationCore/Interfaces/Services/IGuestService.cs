using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services.Home
{
    public interface IGuestService
    {
        Task<List<GuestDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null);
    }
}
