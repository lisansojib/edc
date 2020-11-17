using ApplicationCore;
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

                SELECT Cast(VF.Id As varchar) Id, VF.Name [Text], VT.Name [Desc]
                FROM ValueFields VF
                INNER JOIN ValueFieldTypes VT On VF.TypeId = VT.Id
                WHERE VT.Name In('{ValueFieldTypeNames.GraphTypeName}','{ValueFieldTypeNames.EventTypeName}');";

            var connection = _dbContext.Database.GetDbConnection();

            var data = new PollDTO();
            try
            {
                await connection.OpenAsync();
                var records = await connection.QueryMultipleAsync(query);

                data.OriginList = await records.ReadAsync<Select2Option>();

                var valueFileds = await records.ReadAsync<Select2Option>();
                data.GraphTypeList = valueFileds.Where(x => x.Desc == ValueFieldTypeNames.GraphTypeName);
                data.PanelList = valueFileds.Where(x => x.Desc == ValueFieldTypeNames.EventTypeName);

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
                filterBy = $@"where GraphTypeId like '%{filterBy}%' or Name like '%{filterBy}%' or PanelId like '%{filterBy}%'
                            or OriginId like '%{filterBy}%' or PollDate like '{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Title desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                select Id, GraphTypeId, Name, PanelId, OriginId, PollDate, COUNT(*) OVER () as Total
                from Polls
                {filterBy}
                Group By Id, GraphTypeId, Name, PanelId, OriginId, PollDate
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
