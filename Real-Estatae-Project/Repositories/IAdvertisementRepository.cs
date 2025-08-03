using Real_Estatae_Project.DTO.Advertisement;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IAdvertisementRepository:IRepository<Addvertisement>
    {

        Task<Addvertisement> Add(int id, string ownerID);
        Task<bool> Edit(int id, AdvertisementDTO updatedAd);

        List<AdvertisementDTO> GetAll();
        Addvertisement GetById(int id);

        bool DeleteAds(int id, string userId);

        List<AdvertisementDTO> GetLastTwoAdsByCommunityOwner(string ownerId,string role);
        List<AdsByOwner> GetAllByOwner(string ownerId);
        void Save();
    }
}
