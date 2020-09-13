using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly ISqlQueryRepository<EventDTO> _repository;

        public EventService(ISqlQueryRepository<EventDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<EventDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"where Title like '%{filterBy}%' or Description  like '%{filterBy}%' or Date like '{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Title desc" : orderBy;
            var pageBy = $@"limit {limit} offset {offset}";

            var query = $@"
                select Id, Title, Description, Date
                from announcements
                {filterBy}
                Group By Id, Title, Description, Date
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
