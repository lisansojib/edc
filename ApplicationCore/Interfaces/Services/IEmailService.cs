using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string username, string toEmail, string subject, string message);
    }
}
