using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly ISqlQueryRepository<ParticipantDTO> _repository;

        public ParticipantService(ISqlQueryRepository<ParticipantDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<ParticipantDTO>> GetPagedAsync(int offset, int limit, string filterBy, string orderBy)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"Where Username like '%{filterBy}%' or FirstName  like '%{filterBy}%' or LastName  like '%{filterBy}%'
                            Or Email  like '%{filterBy}%' or Title  like '%{filterBy}%'
                            Or EmailCorp  like '%{filterBy}%' or PhoneCorp  like '%{filterBy}%'
                            Or CompanyName  like '%{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Name desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                ;With 
                P As (
	                Select P.Id, P.Username, P.FirstName, P.LastName, P.Email, P.Verified, P.Phone, P.Mobile
	                , P.Title, P.Active, P.PhotoUrl, P.DateSuspended, P.EmailCorp, P.PhoneCorp, P.LinkedinUrl, C.Name CompanyName
	                From Participants P
	                Inner Join Companies C On P.CompanyId = C.Id
                )

                Select Id, Username, FirstName,	LastName, Email, Verified, Phone, Mobile, Title, Active, PhotoUrl
	                ,DateSuspended,	EmailCorp, PhoneCorp, LinkedinUrl, CompanyName 
                From P
                {filterBy}
                Group By Id, Username, FirstName,	LastName, Email, Verified, Phone, Mobile, Title, Active, PhotoUrl
	                ,DateSuspended,	EmailCorp, PhoneCorp, LinkedinUrl, CompanyName 
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
