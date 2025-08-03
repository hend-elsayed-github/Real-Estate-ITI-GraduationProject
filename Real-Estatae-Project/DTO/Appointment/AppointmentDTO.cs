using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.DTO.Appointment
{
    public class AppointmentDTO
    {
        public string appointmentDate { get; set; }
        public int advertisementId { get; set; }

    }
}
