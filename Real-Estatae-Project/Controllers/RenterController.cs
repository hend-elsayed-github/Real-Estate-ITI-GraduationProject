using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Unit;
using Real_Estatae_Project.Hubs;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenterController : ControllerBase

    {

        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;
        public RenterController(IUserRepository userRepository, INotificationRepository NotificationRepository, IHubContext<NotificationHub> hubContext)
            {
            _userRepository = userRepository;
            _notificationRepository = NotificationRepository;
            _hubContext = hubContext;
        }

        [HttpPost]

        public async Task<IActionResult> startUse([FromForm] RenterSSNDTO renterSSN)

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

            //INotification

            var user = await _userRepository.FindByIdAsync(renterId);
            string userName = user.firstName + " " + user.lastName;
            
            var renter = await _userRepository.FindByIdAsync(renterId);

            var ownerid = renter.RenterUnits.FirstOrDefault()?.ownerId;
            if (ownerid == null) return NotFound("Owner not found.");


            string notificationMessage = $"{userName} registered his unit  using SSN";



            var notification = new Notification
            {
                userId = ownerid,
                sender = userName,
                message = notificationMessage,

            };
            await _notificationRepository.AddAsync(notification);

            // signlR
            await _hubContext.Clients.User(ownerid).SendAsync("ReceiveNotification", new
            {
                message = notificationMessage,
                sender = userName,
                createdAt = DateTime.UtcNow
            });
            
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
