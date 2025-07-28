using System.Net.Mail;
using System.Net;

namespace Real_Estatae_Project.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient(_config["Smtp:Host"])
            {
                Port = int.Parse(_config["Smtp:Port"]),
                Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
                EnableSsl = true,
            };
           
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Smtp:Username"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
