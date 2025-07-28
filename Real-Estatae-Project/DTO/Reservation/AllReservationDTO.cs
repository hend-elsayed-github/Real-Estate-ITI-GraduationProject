using Real_Estatae_Project.CustomAttributes;
using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Real_Estatae_Project.DTO.Reservation
{
    public class AllReservationDTO
    {
        public int id { get; set; }

        public string Status { get; set; }
        public DateTime reservationDate { get; set; } 
        public string Location { get; set; }
        public string name { get; set; }

        public string phoneNumber { get; set; }

        public string email { get; set; }
        public int appointmentId { get; set; }

    }
}
