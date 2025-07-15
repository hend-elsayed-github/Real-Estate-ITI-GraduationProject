using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface ICommunityRepository:IRepository<Community>
    {
        Task<int> Create(Community community);

        void Save();
    }
}
