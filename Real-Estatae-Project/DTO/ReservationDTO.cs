using Real_Estatae_Project.CustomAttributes;
using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.DTO
{
    public class ReservationDTO
    {
        public string name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string phoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        public int appointmentId { get; set; }


    }
}
