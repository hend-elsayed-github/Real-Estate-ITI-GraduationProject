using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IConfiguration Config { get; }

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration Config)
        {
            this.userManager = userManager;
            this.Config = Config;
        }
    }
}
