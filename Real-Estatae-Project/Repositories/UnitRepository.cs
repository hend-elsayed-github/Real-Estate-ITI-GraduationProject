using Real_Estatae_Project.DTO;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ProjectContext Context;
        public UnitRepository(ProjectContext _Context)
        {
            Context = _Context;
        }

        public List<Unit> GetAll()
        {
            throw new NotImplementedException();
        }

        public Unit GetById(int id)
        {
            Unit unit=Context.Units.FirstOrDefault(u=>u.id==id);

            return unit;
        }

        public void Add(Unit entity)
        { 
            throw new NotImplementedException();
        }


        #region update a single unit
        public void Update(int id ,UnitDTO UpdatingRef)
        {
            Unit unitFromDB = GetById(id);
            unitFromDB.price = UpdatingRef.price;
            unitFromDB.description = UpdatingRef.description;
            unitFromDB.status = UpdatingRef.status;
            unitFromDB.type = UpdatingRef.type;
            unitFromDB.image1 = UpdatingRef.image1;
            unitFromDB.image2 = UpdatingRef.image2;
            unitFromDB.image3 = UpdatingRef.image3;
        }
        #endregion


        #region Delete a single unit
        public bool Delete(int id)
        {
            Unit unit = GetById(id);

            if (unit!=null)
            {
                unit.isDeleted = true;
                return true;
            }
            return false;
            
        }
        #endregion


        #region Save
        public void Save()
        {
            Context.SaveChanges();
        } 
        #endregion
    }
}
