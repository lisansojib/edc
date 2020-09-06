using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string username, string toEmail, string subject, string message);
    }
}
