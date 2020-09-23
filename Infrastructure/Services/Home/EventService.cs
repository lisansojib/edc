using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly ISqlQueryRepository<EventDTO> _repository;

        public EventService(ISqlQueryRepository<EventDTO> repository)
        {
            _repository = repository;
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
	                Select E.Id, E.Title, E.Description, E.EventDate, STRING_AGG(SV.Name, ', ') [Speakers], STRING_AGG(SPV.Name, ', ') [Sponsors]
	                From Events E
	                Left Join Speakers S On E.Id = S.EventId
	                Left Join Sponsors SP On E.Id = SP.EventId
	                Left Join ValueFields SV On S.SpeakerId = SV.Id
	                Left Join ValueFields SPV On SP.SponsorId = SPV.Id
	                Group By E.Id, E.Title, E.EventDate, E.Description
                )

                Select Evts.Id, Evts.Title, Evts.Description, Evts.EventDate, Evts.Speakers, Evts.Sponsors 
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
                Select E.Id, E.Title, E.Description, E.EventDate
                    , STRING_AGG(SV.Name, ', ') [Speakers]
                    , STRING_AGG(SPV.Name, ', ') [Sponsors]
                    , COUNT(*) OVER () as Total
	            From Events E
	            Left Join Speakers S On E.Id = S.EventId
	            Left Join Sponsors SP On E.Id = SP.EventId
	            Left Join ValueFields SV On S.SpeakerId = SV.Id
	            Left Join ValueFields SPV On SP.SponsorId = SPV.Id
                Where E.Id = {id}
	            Group By E.Id, E.Title, E.EventDate, E.Description";

            var records = await _repository.RawSqlQueryAsync(query);

            return records.FirstOrDefault();
        }
    }
}
