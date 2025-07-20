using Real_Estatae_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IReservationRepository: IRepository<Reservation>
    {
        List<Reservation> GetAll(string ownerId);
        Reservation GetById(int id);
        bool Delete(int reservationId);

        Task<bool> Edit(int id, string ownerId);
        Task<Reservation> Add(Reservation reservation);
        void Save();
    }
}
