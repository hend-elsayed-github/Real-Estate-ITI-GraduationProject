using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Real_Estatae_Project.CustomAttributes
{
    public class UniqueAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            Reservation reservationFromReq = (Reservation)validationContext.ObjectInstance;
            int appointmentID =(int) value;
            ProjectContext context = new ProjectContext();
            Reservation reservationFromDB = context.Reservations.FirstOrDefault(r=>r.appointmentId==appointmentID);
            if (reservationFromDB == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult( "This appointment is already reserved" );
            }
        }
    
    }
}
