using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SpeakerService : ISpeakerService
    {
        private readonly ISqlQueryRepository<SpeakerDTO> _repository;

        public SpeakerService(ISqlQueryRepository<SpeakerDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<SpeakerDTO>> GetPagedAsync(int offset, int limit, string filterBy, string orderBy)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"Where FirstName like '%{filterBy}%' or LastName like '%{filterBy}%' 
                    or Title like '%{filterBy}%' or CompanyName like '%{filterBy}%' or Website like '%{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by FirstName desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                Select Id, FirstName, LastName, Title, CompanyName, COUNT(*) OVER () as Total
                From Speakers
                {filterBy}
                Group By Id, FirstName, LastName, Title, CompanyName
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
