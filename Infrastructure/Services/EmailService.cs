using ApplicationCore;
using ApplicationCore.Interfaces.Logger;
using ApplicationCore.Interfaces.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(
            IOptions<SmtpSettings> currentConfigs)
        {
            _smtpSettings = currentConfigs.Value;
        }

        public async Task SendEmailAsync(string username, string toEmail, string subject, string messageBody)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.Name, _smtpSettings.Email));
                message.To.Add(new MailboxAddress(username, toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = $@"Greetings {username},",
                    HtmlBody = messageBody
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.EnableSsl);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_smtpSettings.Username, _smtpSettings.Password);

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
