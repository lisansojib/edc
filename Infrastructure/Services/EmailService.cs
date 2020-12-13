using ApplicationCore;
using ApplicationCore.Interfaces.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
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

        public async Task SendEmailAsync(string username, string toEmail, string subject, string messageBody, bool showGreetings = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.Name, _smtpSettings.Email));
                message.Subject = subject;

                var toMailList = toEmail.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in toMailList)
                {
                    message.To.Add(new MailboxAddress(username, toEmail));
                }

                BodyBuilder bodyBuilder;
                if (showGreetings)
                {
                    bodyBuilder = new BodyBuilder
                    {
                        TextBody = $@"Greetings {username},",
                        HtmlBody = messageBody
                    };
                }
                else
                {
                   bodyBuilder = new BodyBuilder { HtmlBody = messageBody};
                }

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
