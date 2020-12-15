using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
	                , P.Title, P.Active, P.PhotoUrl, P.DateSuspended, P.EmailCorp, P.PhoneCorp, P.LinkedinUrl, P.CompanyName
	                From Participants P
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

        public async Task<ChannelAndTeamMembersDTO> GetAllChannelsAsync(int teamMemberId, int eventId)
        {
            var query = $@" 
                ;With 
                E As (
	                Select E.CohortId, E.Id
	                From Events E
	                Left Join Events E2 On E.SessionId = E2.SessionId
	                Where E.Id = {eventId} Or E.SessionId = E2.SessionId
	                Group By E.CohortId, E.Id
                )
                , PT As (
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
	                From E
	                Inner Join Cohorts C On E.CohortId = C.Id
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
                Group By P.UUId, P.FirstName, P.LastName, P.Email

                ;With 
                E As (
	                Select E.Id
	                From Events E
	                Left Join Events E2 On E.SessionId = E2.SessionId
	                Where E.Id = {eventId} Or E.SessionId = E2.SessionId
	                Group By E.CohortId, E.Id
                )

                Select Top(2) EVT.Id, EVT.Title, ISNULL(EVT.Description, '') Description, EVT.EventDate, EVT.ImagePath
	                , C.Name CohortName, VT.Name EventType, ISNULL(CTO.FirstName + ' ' + CTO.LastName, '') CTO
	                , ISNULL(PS.FirstName + ' ' + PS.LastName, '') Speakers, ISNULL(PS.FirstName + ' ' + PS.LastName, '') Sponsors 
                From Events EVT
                Inner Join Cohorts C On EVT.CohortId = C.Id
                Inner Join ValueFields VT On EVT.EventTypeId = VT.Id
                Left Join Participants CTO On EVT.CTOId = CTO.Id
                Left Join Participants PS On EVT.CTOId = PS.Id
                Left Join Participants P On EVT.CTOId = P.Id 
                Left Join EventSpeakers ES On EVT.Id = ES.EventId
                Left Join EventSponsors ESP On EVT.Id = ESP.EventId
                Inner Join E On E.Id = EVT.Id
                Order By VT.SeqNo

                ;With 
                E As (
	                Select E.Id
	                From Events E
	                Left Join Events E2 On E.SessionId = E2.SessionId
	                Where E.Id = {eventId} Or E.SessionId = E2.SessionId
	                Group By E.CohortId, E.Id
                )

                Select R.Id, R.Title, R.Description, R.FilePath, ISNULL(R.PreviewType, 'image') PreviewType, R.EventId 
                From E
                Inner Join EventResources R On E.Id = R.EventId";

            var connection = _dbContext.Database.GetDbConnection();

            var data = new ChannelAndTeamMembersDTO();
            try
            {
                await connection.OpenAsync();
                var records = await connection.QueryMultipleAsync(query);

                data.Channels = await records.ReadAsync<ChannelDTO>();
                data.TeamMembers = await records.ReadAsync<TeamMemberDTO>();
                data.Events = await records.ReadAsync<SessionEventDTO>();

                var resources = await records.ReadAsync<EventResourceDTO>();
                foreach (var item in data.Events)
                {
                    item.Resources = resources.Where(x => x.EventId == item.Id);
                }

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

        public async Task<SessionEventDTO> GetEventDetailsAsync(int eventId)
        {
            var query = $@" 
                ;With 
                EVT As (
	                Select *
	                From Events E
	                Where E.Id = {eventId}
                )

                Select EVT.Id, EVT.Title, ISNULL(EVT.Description, '') Description, EVT.EventDate, EVT.ImagePath
	                , C.Name CohortName, VT.Name EventType, ISNULL(CTO.FirstName + ' ' + CTO.LastName, '') CTO
	                , ISNULL(PS.FirstName + ' ' + PS.LastName, '') Speakers, ISNULL(PS.FirstName + ' ' + PS.LastName, '') Sponsors 
                From EVT EVT
                Inner Join Cohorts C On EVT.CohortId = C.Id
                Inner Join ValueFields VT On EVT.EventTypeId = VT.Id
                Left Join Participants CTO On EVT.CTOId = CTO.Id
                Left Join Participants PS On EVT.CTOId = PS.Id
                Left Join Participants P On EVT.CTOId = P.Id 
                Left Join EventSpeakers ES On EVT.Id = ES.EventId
                Left Join EventSponsors ESP On EVT.Id = ESP.EventId
                Order By VT.SeqNo

                ;With 
                E As (
	                Select E.Id
	                From Events E
	                Left Join Events E2 On E.SessionId = E2.SessionId
	                Where E.Id = {eventId} Or E.SessionId = E2.SessionId
	                Group By E.CohortId, E.Id
                )

                Select R.Id, R.Title, R.Description, R.FilePath, ISNULL(R.PreviewType, 'image') PreviewType, R.EventId 
                From E
                Inner Join EventResources R On E.Id = R.EventId";

            var connection = _dbContext.Database.GetDbConnection();
            try
            {
                await connection.OpenAsync();
                var records = await connection.QueryMultipleAsync(query);

                var data = new SessionEventDTO();
                data = await records.ReadFirstAsync<SessionEventDTO>();
                data.Resources = await records.ReadAsync<EventResourceDTO>();

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
    }    
}
