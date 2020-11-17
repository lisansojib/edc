using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ISqlQueryRepository<AnnouncementDTO> _repository;

        public AnnouncementService(ISqlQueryRepository<AnnouncementDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<AnnouncementDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null)
        {
            if (filterBy.NotNullOrEmpty())
                filterBy = $@"where Title like '%{filterBy}%' or Description  like '%{filterBy}%' or CallAction  like '%{filterBy}%'
                            or LinkUrl like '%{filterBy}%' or Expiration like '{filterBy}%'";
            else
                filterBy = "";

            orderBy = string.IsNullOrEmpty(orderBy) ? "order by Title desc" : orderBy;
            var pageBy = $@"Offset {offset} Rows Fetch Next {limit} Rows Only";

            var query = $@"
                select Id, Title, Description, CallAction, LinkUrl, ImageUrl, Expiration, COUNT(*) OVER () as Total
                from announcements
                {filterBy}
                Group By Id, Title, Description, CallAction, LinkUrl, ImageUrl, Expiration
                {orderBy}
                {pageBy}";

            var records = await _repository.RawSqlQueryAsync(query);

            return records;
        }
    }
}
