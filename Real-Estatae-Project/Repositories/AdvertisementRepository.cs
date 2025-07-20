using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Advertisement;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class AdvertisementRepository: IAdvertisementRepository
    {
        private readonly ProjectContext context;
       public AdvertisementRepository(ProjectContext _context)
        {
            context = _context;     
        }


        #region add advertisement

        public async Task<Addvertisement> Add(Addvertisement newAd)
        {
            if (newAd == null)
            {
                return null;
            }
            await context.Addvertisements.AddAsync(newAd);
            await context.SaveChangesAsync();

            return newAd;
        }
        #endregion

        #region edit advertisement

        public async Task<bool> Edit(int id, AdvertisementDTO updatedAd)
        {
            Addvertisement existingAd = await context.Addvertisements.FindAsync(id);        
            if (existingAd == null)
            {
                return false; // Advertisement not found
            }
            // Update the properties of the existing advertisement  

            existingAd.title = updatedAd.title;
            existingAd.description = updatedAd.description;
            context.SaveChangesAsync();
            return true; // Successfully updated

        }

        #endregion

        ////////////////////

        #region GetAll
        public List<Addvertisement> GetAll()
        {
            return context.Addvertisements
                  .Where(ads => !ads.isDeleted)
                  .Include(ads => ads.Appointments)
                  .Include(ads => ads.unit)
                  .ToList(); ;
        }


        #endregion

        #region GetById

        public Addvertisement GetById(int id)
        {
            return context.Addvertisements
                .Include(ads => ads.unit)
                .Include(ads => ads.Appointments)
                .Where(ads => !ads.isDeleted && ads.id == id)
                .FirstOrDefault();
        }
        #endregion

        #region Delete
        public bool DeleteAds(int id, string userId)
        {
            Addvertisement addvertisement = context.Addvertisements
                    .FirstOrDefault(ads => ads.id == id && ads.userId == userId && !ads.isDeleted);
            if (addvertisement == null)
                return false;

            addvertisement.isDeleted = true;
            Save();
            return true;
        }

        #endregion

        #region LastTwo
        public List<Addvertisement> GetLastTwoAdsByCommunityOwner(int communityId)
        {
            //get owner of community
            string ownerId = context.Communities
                      .Where(c => c.id == communityId)
                      .Select(c => c.ownerId).FirstOrDefault();
            if (string.IsNullOrEmpty(ownerId))
            {
                return new List<Addvertisement>();
            }

            //get last 2ads
            var addvertisement = context.Addvertisements
                     .Where(ads => ads.userId == ownerId && !ads.isDeleted)
                     .OrderByDescending(ads => ads.publishDate)
                     .Take(2).Include(ads => ads.unit).ToList();

            return addvertisement;
        }

        #endregion

        #region Save
        public void Save()
        {
            context.SaveChanges();
        }

        #endregion

    }
}
