using Real_Estatae_Project.DTO.Reservation;
using Real_Estatae_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IReservationRepository: IRepository<Reservation>
    {
        List<AllReservationDTO> GetAll(string ownerId);
        Reservation GetById(int id);
        bool Delete(int reservationId);


        Task<bool> Edit(int id, string ownerId, string status);
        Task<Reservation> Add(Reservation reservation);
        void Save();
    }
}
