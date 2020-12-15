using ApplicationCore.DTOs;
using RestSharp;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IZoomApiService
    {
        Task<IRestResponse> GetUserListAsync(int pageNumber = 1, int pageSize = 30);
        Task<IRestResponse> CreateUserAsync(ZoomUserInfo userInfo);
        Task<IRestResponse> GetListMeetingsAsync(string zoomUserId, int pageNumber = 1, int pageSize = 30);
        Task<IRestResponse> GetMeetingAsync(long meetingId);
        Task<IRestResponse> CreateMeetingAsync(string zoomUserId, CreateingZoomMeetingDTO model);
        Task<IRestResponse> DeleteMeetingAsync(long meetingId);
        string GetJwtToken();
    }
}
