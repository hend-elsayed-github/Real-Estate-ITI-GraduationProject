using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estatae_Project.Models
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
    public class Reservation
    {
        public int id { get; set; }

        public ReservationStatus status { get; set; }= ReservationStatus.Pending;           
        public DateTime reservationDate { get; set; } = DateTime.Now;
        public string name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string phoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        //1-1 appointment
        [ForeignKey("appointment")]
        public int appointmentId { get; set; }
        public virtual Appointment appointment { get; set; }

    }
}
