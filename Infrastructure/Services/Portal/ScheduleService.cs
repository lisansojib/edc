using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services.Portal;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Home
{
    public class ScheduleService: IScheduleService
    {
        private readonly AppDbContext _dbContext;
        private readonly ISqlQueryRepository<PendingSpeakerDTO> _repository;
        public ScheduleService(ISqlQueryRepository<PendingSpeakerDTO> repository, AppDbContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<PendingSpeakerDTO> GetNewPendingSpeakerAsync(int id)
        {
            
            var query = $@"
                Select Cast(Id As varchar) Id, FirstName, LastName, Email, Phone From Participants
                Where Id = {id};

                SELECT Cast(VF.Id As varchar) Id, VF.Name [Text], VT.Name [Desc]
                FROM ValueFields VF
                INNER JOIN ValueFieldTypes VT On VF.TypeId = VT.Id
                WHERE VT.Name In('{ValueFieldTypeNames.EventTypeName}');";

            var connection = _dbContext.Database.GetDbConnection();

            var data = new PendingSpeakerDTO();
            try
            {
                await connection.OpenAsync();
                var records = await connection.QueryMultipleAsync(query);

                var speakerResult = await records.ReadAsync<PendingSpeakerDTO>();
                var speaker = speakerResult.FirstOrDefault();

                data.FirstName = speaker.FirstName;
                data.LastName = speaker.LastName;
                data.Email = speaker.Email;
                data.Phone = speaker.Phone;
                    
                var valueFileds = await records.ReadAsync<Select2Option>();
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
    }
}
