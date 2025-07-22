using Real_Estatae_Project.DTO.Unit;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IUnitRepository : IRepository<Unit>
    {
        List<Unit> GetAll(string ownerId);
        Unit GetById(int id, string ownerId);

        List<Unit> Filter(string? type, string? status,string userId);

        List<Unit> Search(string searchTerm, string userId);
 
        Unit Add(Unit entity);

        Task Update(string ownerId, int id, UnitDTO UpdatingRef);

        bool Delete(string ownerId, int id);

        void Save();
        public int GetCommunityId(string ownerId);



    }
}
