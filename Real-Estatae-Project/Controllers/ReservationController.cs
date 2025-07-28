using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Reservation;
using Real_Estatae_Project.Hubs;
using Real_Estatae_Project.Models;
using Real_Estatae_Project.Repositories;
using Real_Estatae_Project.Services;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(IReservationRepository reservationRepository, INotificationRepository NotificationRepository, IHubContext<NotificationHub> hubContext, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            this.reservationRepository = reservationRepository;
            _notificationRepository = NotificationRepository;
            _hubContext = hubContext;
            _emailService = emailService;
            _userManager = userManager;

        }


        #region GetAll
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public IActionResult GetAll()
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<AllReservationDTO> reservations = reservationRepository.GetAll(ownerId);
            return Ok(reservations);
        }
        #endregion

        #region GetbyId
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var reservation = reservationRepository.GetById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(new { success = true, messsage = "returned successfully", data = reservation });
        }
        #endregion

        #region Delete
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            bool isDelete = reservationRepository.Delete(id);
            if (!isDelete)
            {
                return NotFound();
            }
            return Ok(new { success = true, message = "Reservation deleted" });
        }
        #endregion

        #region Add a reservation, renter or visitor can add a reservation but the owner can not
        [HttpPost]

        public async Task<IActionResult> Add([FromBody] ReservationDTO reservationDTO)
        {
            if (reservationDTO == null)
            {
                return BadRequest(new { message = "Invalid Data" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Reservation reservation = new Reservation
            {
                appointmentId = reservationDTO.appointmentId,
                email = reservationDTO.email,
                name = reservationDTO.name,
                phoneNumber = reservationDTO.phoneNumber
            };

            var addedReservation = await reservationRepository.Add(reservation);
            if (addedReservation == null)
            {
                return BadRequest(new { message = "Failed to add reservation." });
            }

            //INotification

            string userName = reservationDTO.name;
  
            var ownerid = reservation.appointment.ownerId;

            string notificationMessage = $"{userName} booked an appointment for {reservation.reservationDate}";

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

            //   email 
            var ownerUser = await _userManager.FindByIdAsync(ownerid);
            if (ownerUser != null && !string.IsNullOrEmpty(ownerUser.Email))
            {
                string subject = "Appointment Confirmed";
                string message = $"An appointment has been booked for {reservation.appointment} by {reservationDTO.name} his email {reservationDTO.email} phone {reservationDTO.phoneNumber}";
                await _emailService.SendEmailAsync(ownerUser.Email, subject, message);
            }

            return Ok(new { success = true, message = "Reservation added successfully.", data = addedReservation });
        }
        #endregion

        #region edit a reservation by editing its status to confirmed by the owner 
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromQuery] string status)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ownerId == null || !User.IsInRole("Owner"))
            {
                return Unauthorized(new { message = "Unauthorized access." });

            }
            bool isValied = await reservationRepository.Edit(id, ownerId, status);
            if (!isValied)
            {
                return NotFound(new { success = false, message = "Reservation not found or already confirmed." });
            }
            return Ok(new { success = true, message = "Reservation confirmed successfully." });

        }
        #endregion
    }
}
