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
                filterBy = $@"WHERE FirstName  LIKE '%{filterBy}%' OR LastName LIKE '%{filterBy}%'
                            OR Title  LIKE '%{filterBy}%'
                            OR EmailCorp  LIKE '%{filterBy}%' OR PhoneCorp  LIKE '%{filterBy}%'
                            OR CompanyName  LIKE '%{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "ORDER BY Name DESC" : orderBy;
            var pageBy = $@"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";


            var query = $@"
                ;WITH 
                P AS (
	                SELECT P.Id, P.FirstName, P.LastName, P.Phone,
	                , P.Title, P.EmailPersonal, P.EmailCorp, P.PhoneCorp, P.LinkedinUrl, P.CompanyName
	                From Guests
                )

                SELECT Id,FirstName,	LastName, Phone, Title, EmailPersonal,	EmailCorp, PhoneCorp, LinkedinUrl, CompanyName, COUNT(*) OVER () as Total 
                FROM P
                {filterBy} 
                GROUP By Id,FirstName,	LastName,Phone, Title, EmailPersonal, EmailCorp, PhoneCorp, LinkedinUrl, CompanyName 
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
