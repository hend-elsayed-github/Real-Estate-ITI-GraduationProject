using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Appointment;
using Real_Estatae_Project.Models;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class AppointmentRepository: IAppointmentRepository
    {

        private readonly ProjectContext context;
        public AppointmentRepository(ProjectContext _context)
        {
            context = _context;
        }

        #region Get all appointments 
        public async Task<List<Appointment>> GetAll(string id)
        {
            return await context.Appointments
                .Include(a => a.advertisement)
                .Where(a => a.ownerId == id)
                .ToListAsync();
        }
        #endregion

        #region Get all available appointments
        public async Task<List<AppointmentAvDTO>> GetAllAvailable(int _id)
        {
            var today = DateTime.Today;
            List<Appointment> appointments = await context.Appointments
                .Include(a => a.advertisement)
      .Where(a => a.isAvaliable == true && a.advertisement.id == _id && a.appointmentDate.Date >= today)
                .ToListAsync();
            var result = appointments.Select(app => new AppointmentAvDTO
            {
                id = app.id,
                ownerId = app.ownerId,
                advertisementId = app.advertisementId,
                appointmentDate = app.appointmentDate,
                isAvaliable = app.isAvaliable,
            }).ToList();

            return result;
        }
        #endregion


        #region delete an appointment

        public async Task<bool> Delete(int id, string ownerId)
        {
            Appointment appointment = await context.Appointments.Where(a => a.id == id && a.ownerId == ownerId)
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return false; // Appointment not found
            }

            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync();
            return true; // Successfully deleted
        }

        #endregion

        //////////////

        #region addappointment
        public void AddAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);
            Save();

        }
        #endregion

        #region editappointment
        public bool EditAppointment(Appointment appointment)
        {
            var existing = context.Appointments
                      .Include(a => a.reservation)
                      .FirstOrDefault(a => a.id == appointment.id && a.ownerId == appointment.ownerId);

            if (existing == null || existing.reservation != null || !existing.isAvaliable)
            {
                return false;
            }

            existing.appointmentDate = appointment.appointmentDate;
            Save();
            return true;
        }

        #endregion

        #region getbyid

        public Appointment GetById(string ownerId, int id)
        {
            return context.Appointments
                .Where(app => app.ownerId == ownerId && app.id == id).FirstOrDefault();
        }

        #endregion


        #region save
        public void Save()
        {
            context.SaveChanges();
        }

        public List<AllAppointmentDTO> GetAllByOwner(string ownerId)
        {
            List<Appointment> appointments = context.Appointments
                 .Where(ap => ap.ownerId == ownerId && ap.appointmentDate >= DateTime.Today)
                 .Include(ap => ap.advertisement)
                 .ThenInclude(ad => ad.unit).ToList();

            var result = appointments.Select(ap => new AllAppointmentDTO
            {
                id = ap.id,
                dateTime = ap.appointmentDate,
                city = ap.advertisement.unit.city,
                area = ap.advertisement.unit.area,
                buildingNumber = ap.advertisement.unit.buildingNumber,
                street = ap.advertisement.unit.street,
                flatNumber = ap.advertisement.unit.flatNumber

            }).ToList();
            return result;
        }
        #endregion



    }
}
