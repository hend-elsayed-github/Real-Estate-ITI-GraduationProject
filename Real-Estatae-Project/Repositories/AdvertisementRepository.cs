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

        public async Task<Addvertisement> Add(int unitID, string ownerID)
        {
            Unit unitFromDB = await context.Units.FirstOrDefaultAsync(u => u.id == unitID && u.ownerId == ownerID &&
            !u.isDeleted && u.status.Trim().ToLower() == "empty");
            if (unitFromDB == null)
            {
                return null; // Unit not found or is deleted
            }
            //if (unitFromDB.ownerId != ownerID)
            //{
            //    return null; // User does not own this unit
            //}
            Addvertisement newAd = new Addvertisement
            {
                title = unitFromDB.type + " in " + unitFromDB.city,
                description = "Available " + unitFromDB.type + ": " + unitFromDB.description,
                isDeleted = unitFromDB.isDeleted,
                userId = ownerID,
                unitId = unitFromDB.id

            };

            context.Addvertisements.Add(newAd);
            await context.SaveChangesAsync();

            return newAd; // Successfully added advertisement

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
        public List<AdvertisementDTO> GetAll()
        {
            var addvertisements = context.Addvertisements
                  .Where(ads => !ads.isDeleted)
                  .Include(ads => ads.Appointments)
                  .Include(ads => ads.unit)
                  .ThenInclude(u => u.community)
                  .Include(ads => ads.unit)
                  .ThenInclude(u => u.owner)
                  .ToList();

            var result = addvertisements.Select(ad => new AdvertisementDTO
            {
                AdID = ad.id,
                title = ad.title,
                price = ad.unit?.price ?? 0,
                description = ad.description,
                type = ad.unit?.type ?? "Unknown",
                city = ad.unit?.city ?? "Unknown",
                street = ad.unit?.street ?? "Unknown",
                area = ad.unit?.area,
                flatNumber = ad.unit?.flatNumber,
                buildingNumber = ad.unit?.buildingNumber,
                image1 = ad.unit?.image1,
                image2 = ad.unit?.image2,
                image3 = ad.unit?.image3,
                publishDate = ad.publishDate,
                unitId = ad.unitId,
                communityName = ad.unit?.community?.name ?? "Unknown",
                userName = ad.unit?.owner?.UserName ?? "Unknown",
                hasAppointment = ad.Appointments != null && ad.Appointments.Any()
            }).ToList();


            return result;
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
            var advertisement = context.Addvertisements
                .Include(ad => ad.Appointments)
                    .ThenInclude(ap => ap.reservation)
        .FirstOrDefault(ad => ad.id == id && ad.userId == userId && !ad.isDeleted);

            if (advertisement == null)
                return false;

            foreach (var appointment in advertisement.Appointments.ToList())
            {

                if (appointment.reservation != null)
                    context.Reservations.Remove(appointment.reservation);
            }


            if (advertisement.Appointments?.Any() == true)
                context.Appointments.RemoveRange(advertisement.Appointments);


            context.Addvertisements.Remove(advertisement);

            Save();
            return true;
        }

        #endregion

        #region LastTwo
        public List<AdvertisementDTO> GetLastTwoAdsByCommunityOwner(string ownerId, string role)
        {

            if (string.IsNullOrEmpty(ownerId))
            {
                return new List<AdvertisementDTO>();
            }
            int communityId;
            if (role == "Owner")
            {
                var community = context.Communities
                .FirstOrDefault(c => c.ownerId == ownerId);

                if (community == null)
                    return new List<AdvertisementDTO>();

                communityId = community.id;
            }
            else
            {
                var unit = context.Units
               .FirstOrDefault(u => u.renterId == ownerId);

                if (unit == null)
                    return new List<AdvertisementDTO>();

                communityId = unit.communityId;
            }

            // Get last 2 ads
            var advertisements = context.Addvertisements
             .Include(ad => ad.unit)
              .ThenInclude(u => u.community)
               .Where(ad => ad.unit.communityId == communityId && !ad.isDeleted)
               .OrderByDescending(ad => ad.publishDate)
               .Take(2)
               .ToList();

            var result = advertisements.Select(ad => new AdvertisementDTO
            {
                AdID = ad.id,
                title = ad.title,
                price = ad.unit.price,
                description = ad.description,
                type = ad.unit.type,
                city = ad.unit.city,
                street = ad.unit.street,
                area = ad.unit.area,
                flatNumber = ad.unit.flatNumber,
                buildingNumber = ad.unit.buildingNumber,
                image1 = ad.unit.image1,
                image2 = ad.unit.image2,
                image3 = ad.unit.image3,
                publishDate = ad.publishDate,
                unitId = ad.unitId,
                communityName = ad.unit.community.name
            }).ToList();

            return result;
        }


        #endregion

        #region Save
        public void Save()
        {
            context.SaveChanges();
        }

        public List<AdsByOwner> GetAllByOwner(string ownerId)
        {
            List<Addvertisement> addvertisements = context.Addvertisements
                 .Include(ad => ad.unit)
                 .Where(ad => ad.unit.ownerId == ownerId && !ad.isDeleted)
                 .ToList();
            var result = addvertisements.Select(ad => new AdsByOwner
            {
                AdID = ad.id,
                street = ad.unit.street,
                city = ad.unit.city,
                buildingNumber = ad.unit.buildingNumber,
                flatNumber = ad.unit.flatNumber,
                area = ad.unit.area
            }).ToList();
            return result;
        }

        #endregion


    }
}
