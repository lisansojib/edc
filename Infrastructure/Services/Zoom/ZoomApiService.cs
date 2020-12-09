using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ZoomApiService : IZoomApiService
    {
        public async Task<IRestResponse> GetUserListAsync(int pageNumber = 1, int pageSize = 30)
        {
            var token = GetJwtToken();

            var client = new RestClient($"{ZoomSettings.ZOOM_API_ENDPOINT}/users?page_number={pageNumber}&page_size={pageSize}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", $"Bearer {token}");
            return await client.ExecuteAsync(request);
        }

        public async Task<IRestResponse> CreateUserAsync(ZoomUserInfo userInfo)
        {
            var token = GetJwtToken();

            var zoomUser = new CreateZoomUserDTO
            {
                UserInfo = userInfo
            };

            var requestBody = JsonConvert.SerializeObject(zoomUser);

            var client = new RestClient($"{ZoomSettings.ZOOM_API_ENDPOINT}/users");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {token}");
            request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            return await client.ExecuteAsync(request); 
        }

        public async Task<IRestResponse> GetListMeetingsAsync(string zoomUserId, int pageNumber = 1, int pageSize = 30)
        {
            var token = GetJwtToken();
            var client = new RestClient($"{ZoomSettings.ZOOM_API_ENDPOINT}/users/{zoomUserId}/meetings");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {token}");
            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }

        public async Task<IRestResponse> GetMeetingAsync(long meetingId)
        {
            var token = GetJwtToken();
            var client = new RestClient($"{ZoomSettings.ZOOM_API_ENDPOINT}/meetings/{meetingId}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {token}");
            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }

        public async Task<IRestResponse> CreateMeetingAsync(string zoomUserId, CreateingZoomMeetingDTO model)
        {
            var token = GetJwtToken();

            var zoomMeeting = new CreateZoomMeeting
            {
                Topic = model.Topic,
                StartTime = model.StartTime,
                Duration = model.Duration,
                Agenda = model.Agenda
            };

            var requestBody = JsonConvert.SerializeObject(zoomMeeting);

            var client = new RestClient($"{ZoomSettings.ZOOM_API_ENDPOINT}/users/{zoomUserId}/meetings");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {token}");
            request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }

        public string GetJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] symmetricKey = Encoding.ASCII.GetBytes(ZoomSettings.API_SECRET);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = ZoomSettings.API_KEY,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
