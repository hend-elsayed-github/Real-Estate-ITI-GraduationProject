using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenterController : ControllerBase

    {
            IUserRepository _userRepository;
        public RenterController(IUserRepository userRepository)
            {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> startUse([FromBody] RenterSSNDTO renterSSN)
        {
            string renterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(renterId) || !User.IsInRole("Renter"))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
               return BadRequest(ModelState);
            }

            List<Unit> renterUnit = await _userRepository.getUnitBySSN(renterSSN);
            if (renterUnit[0] == null)
            {
                return NotFound(new { message = "Unit not found for the provided SSN." });
            }
            await _userRepository.setRenterCommunity(renterId, renterUnit[0]);
            await _userRepository.setRenterUnit(renterId, renterUnit[0].id);

            
            
            return Ok(new
            {
                message = "Renter is now using the unit.",
                renterUnitsCount = renterUnit.Count,    
                renterUnitsIds = renterUnit.Select(u => u.id).ToList(),
                communityId = renterUnit[0].communityId
            });
        }
    }
}
