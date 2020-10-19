using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PendingSpeakerService : IPendingSpeakerService
    {
        private readonly ISqlQueryRepository<PendingSpeakerDTO> _repository;

        public PendingSpeakerService(ISqlQueryRepository<PendingSpeakerDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<PendingSpeakerDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"WHERE Name LIKE '%{filterBy}%' or Email LIKE '%{filterBy}% or InterestInTopic LIKE '%{filterBy}% or RefferedBy LIKE '%{filterBy}%' or Phone LIKE '%{filterBy}%' or LinkedInUrl LIKE '%{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Name desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                With 
                PS As (
	                Select PS.Id, PS.Username, PS.FirstName + ' ' + PS.LastName Name, PS.Email, PS.InterestInTopic, P.Username ReferredBy, PS.Phone, PS.LinkedInUrl, PS.IsReferrer
	                From PendingSpeakers PS
	                Left Join Participants P On PS.ReferredBy = P.Id
	                WHERE PS.IsRejected = 0 And IsAccepted = 0
                )

                Select Id, Name, Email, InterestInTopic, ReferredBy,Phone, LinkedInUrl,IsReferrer
                From PS
                {filterBy}
                Group By Id, Name, Email, InterestInTopic, ReferredBy,Phone, LinkedInUrl,IsReferrer
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
