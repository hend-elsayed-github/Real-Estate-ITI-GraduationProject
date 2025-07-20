using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.Models;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        private readonly IAppointmentRepository appointmentRepository;

        public AppointmentController(IAppointmentRepository _appointmentRepository)
        {
            this.appointmentRepository = _appointmentRepository;
        }


        #region GetAll all appointments for an owner
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ownerId) || !User.IsInRole("Owner"))
            {
                return Unauthorized(new { message = "Unauthorized access." });
            }

            List<Appointment> appointments = await appointmentRepository.GetAll(ownerId);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound(new { message = "No appointments found." });
            }
            
            return Ok(appointments);

        }
        #endregion


        #region get available appointments by ad id 
        [HttpGet("available/{id}")]

        public async Task<IActionResult> GetAllAvailable(int id)
        {

            List<Appointment> appointments = await appointmentRepository.GetAllAvailable(id);
            if (appointments == null || appointments.Count == 0)
            {
                return NotFound(new { message = "No appointments found." });
            }

            return Ok(appointments);

        }

        #endregion


        #region delete an appointment
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ownerId) || !User.IsInRole("Owner"))
            {
                return Unauthorized(new { message = "Unauthorized access." });
            }
            bool isDeleted = await appointmentRepository.Delete(id, ownerId);

            if (!isDeleted)
            {
                return NotFound(new { message = "Appointment not found or you do not have permission to delete it." });
            }
            return Ok(new { message = "Appointment deleted successfully." });
        }

        #endregion

        /////////////////////

        #region addappointment
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public IActionResult Add(AppointmentDTO appointmentDTO)
        {
            string _ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Appointment appointment = new Appointment
            {
                ownerId = _ownerId,
                appointmentDate = appointmentDTO.appointmentDate,
                isAvaliable = true,
                advertisementId = appointmentDTO.advertisementId
            };
            appointmentRepository.AddAppointment(appointment);
            return Ok(new { success = true, message = "Appointment slot added successfully" });
        }
        #endregion


        #region Edit 

        [HttpPost("{id}")]
        [Authorize(Roles = "Owner")]
        public IActionResult Edit(int id, AppointmentDTO appointmentDTO)
        {
            string _ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Appointment updateAppointment = new Appointment
            {
                id = id,
                appointmentDate = appointmentDTO.appointmentDate,
                ownerId = _ownerId
            };
            bool success = appointmentRepository.EditAppointment(updateAppointment);
            if (!success)
            {
                return BadRequest(new { success = false, message = "Cannot edit a reserved or unavailable appointment." });

            }

            return Ok(new { success = true, message = " update success" });

        }
        #endregion

    }
}
