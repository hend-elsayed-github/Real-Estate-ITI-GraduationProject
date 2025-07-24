using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO.Reservation;
using Real_Estatae_Project.Models;
using Real_Estatae_Project.Repositories;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
        }

        #region GetAll
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public IActionResult GetAll()
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Reservation> reservations = reservationRepository.GetAll(ownerId);
            return Ok(new { success = true, messsage = "returned successfully", data = reservations });
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
            return Ok(new { success = true, message = "Reservation added successfully.", data = addedReservation });
        }
        #endregion

        #region edit a reservation by editing its status to confirmed by the owner 
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ownerId == null || !User.IsInRole("Owner"))
            {
                return Unauthorized(new { message = "Unauthorized access." });

            }
            bool isValied=await reservationRepository.Edit(id, ownerId);
            if (!isValied)
            {
                return NotFound(new { message = "Reservation not found or already confirmed." });
            }
            return Ok(new { success = true, message = "Reservation confirmed successfully." });

        }
        #endregion
    }
}
