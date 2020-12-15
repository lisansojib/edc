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
    public class EventService : IEventService
    {
        private readonly ISqlQueryRepository<EventDTO> _repository;
        private readonly AppDbContext _dbContext;

        public EventService(ISqlQueryRepository<EventDTO> repository
            , AppDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<List<EventDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"where Title like '%{filterBy}%' or Description like '%{filterBy}%' 
                    or Speakers like '%{filterBy}%' or Sponsors like '%{filterBy}%' or EventDate like '{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Title desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                ;With
                Evts As (
	                Select E.Id, C.Name CohortName, E.Title, E.Description, E.MeetingId, E.MeetingPassword
						, E.EventDate, STRING_AGG(SV.FirstName + ' ' + SV.LastName + ' (' + SV.Title + ')', ', ') [Speakers], STRING_AGG(SPV.CompanyName, ', ') [Sponsors]
                    From Events E
					Inner Join Cohorts C On E.CohortId = C.Id
                    Left Join EventSpeakers S On E.Id = S.EventId
                    Left Join EventSponsors SP On E.Id = SP.EventId
                    Left Join Speakers SV On S.SpeakerId = SV.Id
                    Left Join Sponsors SPV On SP.SponsorId = SPV.Id
                    Group By E.Id, E.Title, E.EventDate, E.Description, E.MeetingId, E.MeetingPassword, C.Name
                )

                Select Id, Title, Description, EventDate, Speakers, Sponsors, MeetingId, MeetingPassword, COUNT(*) OVER () as Total 
                From Evts
                {filterBy}
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }

        public async Task<EventDTO> GetByIdAsync(int id)
        {
            var query = $@"
                Select E.*
                    , STRING_AGG(SV.Name, ', ') [Speakers]
                    , STRING_AGG(SPV.Name, ', ') [Sponsors]
	            From Events E
	            Left Join Speakers S On E.Id = S.EventId
	            Left Join Sponsors SP On E.Id = SP.EventId
	            Left Join ValueFields SV On S.SpeakerId = SV.Id
	            Left Join ValueFields SPV On SP.SponsorId = SPV.Id
                Where E.Id = {id}
	            Group By E.Id, E.Title, E.EventDate, E.Description";

            return await _repository.FirstOrDefaultAsync(query);
        }

        public async Task<EventDTO> GetNewAsync()
        {
            var query = @"
                Select Cast(Id As varchar) Id, Name [text] From Cohorts

                Select Cast(Id As varchar) Id, FirstName + ' ' + LastName + ' (' + Title + ')' [Text]
                From Speakers;

                Select Cast(Id As varchar) Id, CompanyName [Text], ContactPerson + ' (' + ContactPersonEmail + ')' [Desc] 
                From Sponsors;

                Select CAST(Id As varchar) [id], Name [text] 
                From ValueFields
                Where TypeId = 4;

                Select CAST(Id As varchar) [id], FirstName + ' ' + LastName + '(' + Email + ')' [text] 
                From Participants;";

            var connection = _dbContext.Database.GetDbConnection();

            var data = new EventDTO();
            try
            {
                await connection.OpenAsync();
                var records = await connection.QueryMultipleAsync(query);

                data.CohortList = await records.ReadAsync<Select2Option>();
                data.SpeakerList = await records.ReadAsync<Select2Option>();
                data.SponsorList = await records.ReadAsync<Select2Option>();
                data.EventTypeList = await records.ReadAsync<Select2Option>();
                data.CTOList = await records.ReadAsync<Select2Option>();
                data.PresenterList = data.CTOList;

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
