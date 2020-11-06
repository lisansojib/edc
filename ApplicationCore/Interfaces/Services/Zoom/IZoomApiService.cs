using ApplicationCore.Entities;
using RestSharp;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IZoomApiService
    {
        Task<IRestResponse> CreateUserAsync(User model);
        string GetJwtToken();
    }
}
