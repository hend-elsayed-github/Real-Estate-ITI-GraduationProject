using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO.Admin;
using Real_Estatae_Project.DTO.Auth;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> userManager;
        private IConfiguration Config { get; }
        private IAdminRepository adminRepository;

        public AdminController(UserManager<ApplicationUser> userManager, IConfiguration Config, IAdminRepository _adminRepository)
        {
            this.userManager = userManager;
            this.Config = Config;
            adminRepository = _adminRepository;

        }


        #region Registeration

        [HttpPost("Register")] // api/Admin/Register
        public async Task<IActionResult> Register([FromBody] AdminDTO adminFromRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ApplicationUser user = new ApplicationUser
            {
                UserName = adminFromRequest.userName,
                Email = adminFromRequest.email,
                firstName = "Admin",
                lastName = "Admin",
            };

            IdentityResult result = await userManager.CreateAsync(user, adminFromRequest.password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("Errors", error.Description);

                return BadRequest(ModelState);
            }



            await userManager.AddToRoleAsync(user, "Admin");

            

            return Ok(new { message = "Admin registered successfully" });
        }

        #endregion


        #region Get All Users
        [HttpGet("Users")]
        public async Task<IActionResult> getAll()
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var users =await adminRepository.GetAll();
            return Ok(new { message = "success", data = users });
        }

        #endregion


        #region Get All Owners
        [HttpGet("Owners")]
        public async Task<IActionResult> getAllOwners()
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
            var owners =await adminRepository.GetAllOwners();
            return Ok(new { message = "success", data = owners });
        }

        #endregion


        #region Get All Renters
        [HttpGet("Renters")]
        public async Task<IActionResult> getAllRenters()
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var renters =await adminRepository.GetAllRenters();
            return Ok(new { message = "success", data = renters });
        }

        #endregion


        #region Get general numbers
        [HttpGet("Numbers")]
        public async Task<IActionResult> getNumbers()
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
                var numbers = await adminRepository.GeneralNumbers();
                return Ok(numbers);

            }
        #endregion


        #region get profits per month
        [HttpGet("profitsPerMonth")]
        public async Task<IActionResult> profitsPerMonth()
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
            var profits = await adminRepository.GetProfitMonth();
            return Ok(new { maessage = "success" , data=profits});

        }
        #endregion

        #region Transfer
        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransferDTO transfer)
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(new { message = "old owner id and new one are required" });
            }

            try
            {
                await adminRepository.Transfer(transfer.oldOwnerId, transfer.newOwnerId);
                return Ok(new { message = "success" });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "An error occurred during transfer.", error = ex.Message });
            }
        }
        #endregion

        #region ads vs reservation

        [HttpGet("adsVsReservations")]
        public ActionResult<List<AdsVsReservationsDTO>> GetAdsVsReservations()
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
            var result = adminRepository.GetMonthlyAdsVsReservations();
            return Ok(result);
        }
        #endregion

        #region profitperCommunity
        [HttpGet("profitperCommunity")]
        public async Task<IActionResult> GetProfitPerCommunity()
        {
            string adminId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
            var data= await adminRepository.GetProfitPerCommunity();
            return Ok(data);
        }
        #endregion

        #region AllReservation
        [HttpGet("AllReservation")]
        public ActionResult<List<AllReserDTO>> GetReservation()
        {
            string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(adminId) || !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }
            var result = adminRepository.GetReservation();
            return Ok(result);
        }
        #endregion
    }
}
