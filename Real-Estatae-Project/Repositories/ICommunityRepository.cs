using Real_Estatae_Project.DTO.Community;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface ICommunityRepository:IRepository<Community>
    {
        Task<int> Create(Community community);
        int GetCommunityId(string ownerId);
        string GetName(int commId);
        bool Update(string ownerId, CommunityInfoDTO newName);
        List<compLocationDTO> Get(string ownerId);
        void Save();
    }
}
