using ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface ITeamService
    {
        Task<List<ParticipantTeamDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null);

        Task<List<MyTeamDTO>> GetMyTeamsAsync(int userId);

        Task<List<MyTeamMemberDTO>> GetAllTeamMembersAsync(int userId);
    }
}
