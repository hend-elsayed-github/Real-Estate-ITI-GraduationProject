using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO.Notification;
using Real_Estatae_Project.Repositories;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _NotificationRepository;
        public NotificationController(INotificationRepository NotificationRepository)
        {
            _NotificationRepository = NotificationRepository;
        }

        [HttpGet]

        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var notifications = await _NotificationRepository.GetUserNotifications(userId);

            var notificationsdto = notifications.Select(n => new NotificationDTO
            {
                Id = n.id,
                Massage = n.message,
                isRead = n.isRead,
            });
            return Ok(notifications);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {


            await _NotificationRepository.MarkAsRead(id);
            return NoContent();
        }





    }
}