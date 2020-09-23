using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISqlQueryRepository<SponsorDTO> _repository;

        public SponsorService(ISqlQueryRepository<SponsorDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<SponsorDTO>> GetPagedAsync(int offset, int limit, string filterBy, string orderBy)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"Where CompanyName like '%{filterBy}%' or ContactPerson like '%{filterBy}%' or ContactPersonEmail like '%{filterBy}%'
                    or ContactPersonPhone like '%{filterBy}%' or Website like '%{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by CompanyName desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                Select Id, CompanyName, ContactPerson, ContactPersonEmail, ContactPersonPhone
                    , Description, LogoUrl, Website, COUNT(*) OVER () as Total 
                From Sponsors
                {filterBy}
                Group By Id, CompanyName, ContactPerson, ContactPersonEmail, ContactPersonPhone
                    , Description, LogoUrl, Website
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
