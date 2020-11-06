using ApplicationCore;
using ApplicationCore.Entities;
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
        public async Task<IRestResponse> CreateUserAsync(User model)
        {
            var token = GetJwtToken();

            var requestBody = JsonConvert.SerializeObject(
                new
                {
                    action = "create" ,
                    user_info = new
                    {
                        email = model.Email,
                        type = 1,
                        first_name = model.FirstName,
                        last_name = model.LastName
                    }
                });

            var client = new RestClient($"{ZoomSettings.ZOOM_API_ENDPOINT}/users");
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
