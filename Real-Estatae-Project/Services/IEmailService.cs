namespace Real_Estatae_Project.Services
{
    public interface IEmailService
    {
        //public  Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
    }
}
