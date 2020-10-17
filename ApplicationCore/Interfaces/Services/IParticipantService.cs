using ApplicationCore.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IParticipantService
    {
        Task<List<ParticipantDTO>> GetPagedAsync(int offset = 0, int limit = 10, string filterBy = null, string orderBy = null);

        Task<ChannelAndTeamMembersDTO> GetAllChannelsAsync(int teamMemberId);

        Task<IEnumerable<TeamMemberDTO>> GetTeamMembersAsync(string teamName);

    }
}
