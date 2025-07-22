using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.Repositories;

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


    }
}
