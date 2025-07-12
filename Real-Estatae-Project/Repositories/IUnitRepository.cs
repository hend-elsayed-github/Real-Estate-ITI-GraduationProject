using Real_Estatae_Project.DTO;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IUnitRepository : IRepository<Unit>
    {
        List<Unit> GetAll(string ownerId);
        Unit GetById(int id, string ownerId);

        List<Unit> GetByType(string type, string? userId, string? role);
        List<Unit> GetByStatus(string status, string? userId, string? role);
        Unit Add(Unit entity);

        Task Update(string ownerId, int id, UnitDTO UpdatingRef);

        bool Delete(string ownerId, int id);

        void Save();
        public int GetCommunityId(string ownerId);



    }
}
