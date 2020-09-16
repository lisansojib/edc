using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TeamService : ITeamService
    {
        private readonly ISqlQueryRepository<ParticipantTeamDTO> _repository;

        public TeamService(ISqlQueryRepository<ParticipantTeamDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<ParticipantTeamDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"where Name like '%{filterBy}%' or Description '%{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Title desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                ;With 
                PT As (
	                Select T.Id, T.Name, T.Description, STRING_AGG(P.Email, ',') [Participants] 
	                From Teams T
	                Left Join ParticipantTeams PT On T.Id = PT.TeamId
	                Left Join Participants P On P.Id = PT.TeamMemberId
	                Group By T.Id, Name, Description
                )

                Select Id, Name, Description, Participants
                From PT
                {filterBy}
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
