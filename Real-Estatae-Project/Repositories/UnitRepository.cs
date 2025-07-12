using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        #region GetAll
        public List<Unit> GetAll(string ownerId)
        {
            return [.. Context.Units
                .Include(u => u.Bills)
                .Include(u => u.Reviews)
                .Include(u => u.Maintenances)
                .Where(u => u.ownerId == ownerId && !u.isDeleted)];
        }
        #endregion

        #region GetById
        public Unit GetById(int id, string ownerId)
        {
            return Context.Units
                 .FirstOrDefault(u => u.id == id && u.ownerId == ownerId && !u.isDeleted);
        }
        #endregion
        #region GetByType

        #endregion

        #region AddUnit
        public Unit Add(Unit entity)
        {
            Context.Units.Add(entity);
            Save();
            return entity;

        }
        #endregion


        #region update a single unit
        public async Task Update(string ownerId, int id, UnitDTO UpdatingRef)
        {
            Unit unitFromDB = GetById(id, ownerId);

            unitFromDB.price = UpdatingRef.price;
            unitFromDB.description = UpdatingRef.description;
            unitFromDB.status = UpdatingRef.status;
            unitFromDB.type = UpdatingRef.type;

            List<IFormFile?> images = new() { UpdatingRef.image1, UpdatingRef.image2, UpdatingRef.image3 };
            string?[] currentImagePaths = { unitFromDB.image1, unitFromDB.image2, unitFromDB.image3 };

            for (int i = 0; i < images.Count; i++)
            {
                IFormFile? newImage = images[i];

                if (newImage != null && newImage.Length > 0)
                {
                    // i need to delete the old image if exists
                    if (!string.IsNullOrEmpty(currentImagePaths[i]))
                    {
                        string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", currentImagePaths[i]);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    // Save new image 
                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
                    string newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", newFileName);

                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await newImage.CopyToAsync(stream);
                    }

                    // update the path
                    currentImagePaths[i] = newFileName;
                }
            }

            // update the patshs in database
            unitFromDB.image1 = currentImagePaths[0];
            unitFromDB.image2 = currentImagePaths[1];
            unitFromDB.image3 = currentImagePaths[2];
        }

        #endregion


        #region Delete a single unit
        public bool Delete(string ownerId, int id)
        {
            Unit unit = GetById(id, ownerId);

            if (unit != null)
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

        #region GetbyType
        public List<Unit> GetByType(string type, string? userId, string? role)
        {
            if (role == "Owner")
            {
                return [..Context.Units
                    .Where(u => u.ownerId == userId && u.type.ToLower() == type.ToLower() && !u.isDeleted)];
            }
            return [..Context.Units
                    .Where(u=> u.type.ToLower() == type.ToLower() && !u.isDeleted)];

        }
        #endregion


        #region GetbyStatus
        public List<Unit> GetByStatus(string status, string? userId, string? role)
        {
            if (role == "Owner")
            {
                return [..Context.Units
                    .Where(u => u.ownerId == userId && u.status.ToLower() == status.ToLower() && !u.isDeleted)];
            }
            return [..Context.Units
                    .Where(u=> u.status.ToLower() == status.ToLower() && !u.isDeleted)];

        }
        #endregion


        public int GetCommunityId(string ownerId)
        {
            return Context.Communities
                .Where(c => c.ownerId == ownerId)
                .Select(c => c.id)
                .FirstOrDefault();
        }

    }
}
