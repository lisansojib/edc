﻿using ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IEventService
    {
        Task<List<EventDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null);
        Task<EventDTO> GetByIdAsync(int id);
        Task<EventDTO> GetNewAsync();
    }
}
