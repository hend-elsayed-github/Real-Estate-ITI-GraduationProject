using Real_Estatae_Project.DTO.Appointment;
using Real_Estatae_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IAppointmentRepository: IRepository<Appointment>
    {
        Task<List<Appointment>> GetAll(string id);
        Task<List<AppointmentAvDTO>> GetAllAvailable(int id);
        Task<bool> Delete(int id, string ownerId);
        List<AllAppointmentDTO> GetAllByOwner(string ownerId);
        void AddAppointment(Appointment appointment);
        bool EditAppointment(Appointment appointment);
        Appointment GetById(string ownerId, int id);
        void Save();
    }
}
