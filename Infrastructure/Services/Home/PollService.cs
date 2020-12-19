using ApplicationCore;
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
    public class PollService : IPollService
    {
        private readonly ISqlQueryRepository<PollDTO> _repository;
        private readonly AppDbContext _dbContext;
        public PollService(ISqlQueryRepository<PollDTO> repository, AppDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<PollDTO> GetNewAsync()
        {
            var query = $@"
                Select Cast(Id As varchar) Id, Name [text] From Cohorts;

                ;SELECT Cast(VF.Id As varchar) Id, VF.Name [Text], VT.Name [Desc]
                FROM ValueFields VF
                INNER JOIN ValueFieldTypes VT On VF.TypeId = VT.Id
                WHERE VF.Active = 1 And VT.Name In('{ValueFieldTypeNames.GraphTypeName}');

                ;SELECT Cast(E.Id As varchar) Id, E.Title [text]
                FROM Events E";

            var connection = _dbContext.Database.GetDbConnection();

            var data = new PollDTO();
            try
            {
                await connection.OpenAsync();
                var records = await connection.QueryMultipleAsync(query);

                data.CohortList = await records.ReadAsync<Select2Option>();
                data.GraphTypeList = await records.ReadAsync<Select2Option>();
                data.EventList = await records.ReadAsync<Select2Option>();

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

        public async Task<List<PollDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"where Name like '%{filterBy}%' or Event like '%{filterBy}%' 
                            or GraphType like '%{filterBy}%' or Cohort like '{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Name desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                With 
                P As(
	                Select P.Id, P.Name, E.Title [Event], Graph.Name GraphType, C.Name Cohort
	                From Polls P
	                Inner Join [Events] E On P.EventId = E.Id
	                Inner Join ValueFields Graph On P.GraphTypeId = Graph.Id
	                Inner Join Cohorts C On E.CohortId = C.Id
                )

                Select Id, Name, Event, GraphType, Cohort, COUNT(*) OVER () as Total
                From P                
                {filterBy}
                Group By Id, Name, Event, GraphType, Cohort
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
