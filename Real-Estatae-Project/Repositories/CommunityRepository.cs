using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class CommunityRepository:ICommunityRepository
    {
        private readonly ProjectContext Context;
        public CommunityRepository(ProjectContext _Context)
        {
            Context = _Context;
        }

        public void Create(Community community)
        {
            Context.Communities.Add(community);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
