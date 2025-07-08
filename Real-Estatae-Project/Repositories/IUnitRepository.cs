using Real_Estatae_Project.DTO;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IUnitRepository : IRepository<Unit>
    {
        List<Unit> GetAll();
        Unit GetById(int id);
        void Add(Unit entity);

        void Update(int id,UnitDTO entity);

        bool Delete(int id);

        void Save();


    }
}
