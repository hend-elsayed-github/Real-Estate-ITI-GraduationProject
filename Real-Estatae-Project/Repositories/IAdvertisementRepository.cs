using Real_Estatae_Project.DTO.Advertisement;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IAdvertisementRepository:IRepository<Addvertisement>
    {
        Task<AdvertisementDTO> Add(int id , string ownerID);
        Task<bool> Edit(int id, AdvertisementDTO updatedAd);

        List<Addvertisement> GetAll();
        Addvertisement GetById(int id);

        bool DeleteAds(int id, string userId);

        List<Addvertisement> GetLastTwoAdsByCommunityOwner(int communityId);
        void Save();
    }
}
