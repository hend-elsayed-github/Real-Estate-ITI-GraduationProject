using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.Services;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail(string email)
        {
            await _emailService.SendEmailAsync(
                toEmail: email,
                subject: "Welcome to Our App",
                htmlBody: "<h1>Hi there!</h1><p>Thanks for joining us 🎉</p>"
            );

            return Ok("Email sent successfully.");
        }
    }
}
