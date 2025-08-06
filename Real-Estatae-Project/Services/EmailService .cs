using Microsoft.Extensions.Options;
using Real_Estatae_Project.DTO.Email;
using System.Net;
using System.Net.Mail;

namespace Real_Estatae_Project.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var emailSettings = _config.GetSection("EmailSettings").Get<EmailSettings>();

            var message = new MailMessage();
            message.From = new MailAddress(emailSettings.SenderEmail, emailSettings.SenderName);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = htmlBody;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient(emailSettings.SmtpHost, emailSettings.SmtpPort))
            {
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                await smtp.SendMailAsync(message);
            }

        }

        
    }
}
