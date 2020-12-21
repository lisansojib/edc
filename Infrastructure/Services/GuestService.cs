using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Home
{
    public class GuestService : IGuestService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlQueryRepository<GuestDTO> _repository;

        public GuestService(AppDbContext dbContext, ISqlQueryRepository<GuestDTO> repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<List<GuestDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"WHERE FirstName LIKE '%{filterBy}%' OR LastName LIKE '%{filterBy}%' OR Title LIKE '%{filterBy}%' 
                            OR Email LIKE '%{filterBy}%' OR EmailPersonal LIKE '%{filterBy}%'
                            OR EmailCorp LIKE '%{filterBy}%' OR PhoneCorp LIKE '%{filterBy}%'
                            OR CompanyName  LIKE '%{filterBy}%'";
            else filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "ORDER BY Id DESC" : orderBy;
            var pageBy = $@"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";

            var query = $@"
                ;WITH 
                G AS (
	                SELECT G.Id, G.FirstName, G.LastName, G.EmailPersonal, G.EmailCorp, G.PhonePersonal, G.PhoneCorp, G.LinkedinUrl
		                , G.CompanyName, G.Title, G.Email
	                From Guests G
                )

                SELECT Id, FirstName, LastName, EmailPersonal, EmailCorp, PhonePersonal, PhoneCorp, LinkedinUrl
	                , CompanyName, Title, Email, COUNT(*) OVER () as Total 
                FROM G
                {filterBy} 
                GROUP By Id, FirstName, LastName, EmailPersonal, EmailCorp, PhonePersonal, PhoneCorp, LinkedinUrl
	                , CompanyName, Title, Email 
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
