using ApplicationCore.DTOs;
using RestSharp;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IZoomApiService
    {
        Task<IRestResponse> GetUserListAsync();
        Task<IRestResponse> CreateUserAsync(ZoomUserInfo userInfo);
        Task<IRestResponse> GetListMeetings(string zoomUserId);
        Task<IRestResponse> CreateMeetingAsync(string zoomUserId, CreateingZoomMeetingDTO model);
        string GetJwtToken();
    }
}
