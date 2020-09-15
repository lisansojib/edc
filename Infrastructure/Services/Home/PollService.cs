using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PollService : IPollService
    {
        private readonly ISqlQueryRepository<PollDTO> _repository;

        public PollService(ISqlQueryRepository<PollDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<PollDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"where GraphType like '%{filterBy}%' or Name like '%{filterBy}%' or Panel like '%{filterBy}%'
                            or Origin like '%{filterBy}%' or PollDate like '{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Title desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                select Id, GraphType, Name, Panel, Origin, PollDate
                from Polls
                {filterBy}
                Group By Id, Id, GraphType, Name, Panel, Origin, PollDate
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
