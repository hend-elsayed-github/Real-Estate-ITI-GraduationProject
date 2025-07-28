using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.DTO.Appointment
{
    public class AppointmentAvDTO
    {
        public int id { get; set; }
        public DateTime appointmentDate { get; set; }
        public bool isAvaliable { get; set; }

        public string ownerId { get; set; }
        public int advertisementId { get; set; }
    }
}
