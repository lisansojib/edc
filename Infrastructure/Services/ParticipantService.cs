using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlQueryRepository<ParticipantDTO> _repository;

        public ParticipantService(ISqlQueryRepository<ParticipantDTO> repository
           , AppDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
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
	                ,DateSuspended,	EmailCorp, PhoneCorp, LinkedinUrl, CompanyName, COUNT(*) OVER () as Total 
                From P
                {filterBy}
                Group By Id, Username, FirstName,	LastName, Email, Verified, Phone, Mobile, Title, Active, PhotoUrl
	                ,DateSuspended,	EmailCorp, PhoneCorp, LinkedinUrl, CompanyName 
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }

        public async Task<ChannelAndTeamMembersDTO> GetAllChannelsAsync(int teamMemberId)
        {
            var query = $@" 
                ;With 
                PT As (
	                Select * 
	                From ParticipantTeams
	                Where TeamMemberId = {teamMemberId}
                )

                Select C.Name, IsCohort
                From (
	                Select T.Name, 0 IsCohort 
	                From PT
	                Inner Join Teams T On PT.TeamId = T.Id
	                Union
	                Select C.Name, 1 IsCohort
	                From dbo.Events E
	                Inner Join Cohorts C On E.CohortId = C.Id
	                --Where E.Id In ()
                    Group By C.Name
                ) C
                Order By IsCohort Desc;

                ;With 
                PT As (
	                Select * 
	                From ParticipantTeams
	                Where TeamMemberId = {teamMemberId}
                )

				Select P.UUId, P.FirstName + ' ' + P.LastName Name, P.Email 
				From ParticipantTeams T
				Left Join PT On T.TeamId = PT.TeamId
				Left Join Participants P ON T.TeamMemberId = P.Id
                Group By P.UUId, P.FirstName, P.LastName, P.Email";

            var connection = _dbContext.Database.GetDbConnection();

            var data = new ChannelAndTeamMembersDTO();
            try
            {
                await connection.OpenAsync();
                var records = await connection.QueryMultipleAsync(query);

                data.Channels = await records.ReadAsync<ChannelDTO>();
                data.TeamMembers = await records.ReadAsync<TeamMemberDTO>();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<IEnumerable<TeamMemberDTO>> GetTeamMembersAsync(string teamName)
        {
            var query = $@"
            Select P.UUId, P.FirstName + ' ' + P.LastName Name, P.Email 
            From ParticipantTeams PT
            Inner Join Teams T On PT.TeamId = T.TeamId
            Inner Join Participants P ON PT.TeamMemberId = P.Id
            Where T.Name = '{teamName}'
            Group By P.UUId, P.FirstName, P.LastName, P.Email";
            return await _repository.RawSqlQueryAsync<TeamMemberDTO>(query);
        }
    }    
}
