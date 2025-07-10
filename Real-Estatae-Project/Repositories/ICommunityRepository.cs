using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface ICommunityRepository:IRepository<Community>
    {
        void Create(Community community);

        void Save();
    }
}
