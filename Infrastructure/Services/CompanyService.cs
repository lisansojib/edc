using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ISqlQueryRepository<CompanyDTO> _repository;

        public CompanyService(ISqlQueryRepository<CompanyDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<CompanyDTO>> GetPagedAsync(int offset, int limit, string filterBy, string orderBy)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"Where Name like '%{filterBy}%' or Phone  like '%{filterBy}%' or Website  like '%{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Name desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                Select Id, Name, Address, Phone, LogoUrl, Website, COUNT(*) OVER () as Total 
                From Companies
                {filterBy}
                Group By Id, Name, Address, Phone, LogoUrl, Website
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
