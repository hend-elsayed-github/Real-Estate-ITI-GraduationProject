using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class ReservationRepository: IReservationRepository
    {
        private readonly ProjectContext context;
        public ReservationRepository(ProjectContext _context)
        {
            this.context = _context;
        }


        #region getAll
        public List<Reservation> GetAll(string ownerId)
        {
            return context.Reservations.Where(r => r.appointment.ownerId == ownerId).ToList();
        }
        #endregion

        #region GetById

        public Reservation GetById(int id)
        {
            return context.Reservations.Where(r => r.id == id).FirstOrDefault();
        }

        #endregion


        #region   delete
        public bool Delete(int reservationId)
        {
            var existing = GetById(reservationId);
            if (existing == null)
            {
                return false;
            }
            var appointment = context.Appointments.Find(existing.appointmentId);
            if (appointment != null)
            {
                appointment.isAvaliable = true;
                appointment.reservation = null;
            }
            context.Reservations.Remove(existing);
            Save();
            return true;
        }

        #endregion


        #region Save
        public void Save()
        {
            context.SaveChanges();
        }
        #endregion

        #region add a reservation
        // renter or visitor can add a reservation but the owner can not
        public async Task< Reservation> Add(Reservation reservation)

        {
            if (reservation == null)
            {
                return null;
            }
            using var transaction = await context.Database.BeginTransactionAsync();
            var appointment = await context.Appointments.Where(a => a.id == reservation.appointmentId && a.isAvaliable).FirstOrDefaultAsync();          
            if (appointment == null || !appointment.isAvaliable)
            {
                return null; // Appointment not found or not available
            }

            appointment.isAvaliable = false; // Mark the appointment as not available
            appointment.reservation = reservation; // Link the reservation to the appointment
            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (reservation);

        }
        #endregion

        #region Edit a reservation 

        public async Task<bool> Edit(int id, string ownerId)
        {
            var existingReservation = await context.Reservations.Where(r => r.id == id && r.appointment.ownerId == ownerId).FirstOrDefaultAsync();      
            if (existingReservation == null)
            {
                return false; // Reservation not found
            }
            // Update the properties of the existing reservation
            existingReservation.status=ReservationStatus.Confirmed; // Example of updating the status to confirmed
            await context.SaveChangesAsync();
            return true; // Successfully updated
        }

        #endregion


    }
}
