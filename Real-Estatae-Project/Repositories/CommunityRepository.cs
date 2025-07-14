using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class CommunityRepository:ICommunityRepository
    {
        private readonly ProjectContext _context;
        public CommunityRepository(ProjectContext _Context)
        {
            _context = _Context;
        }



        public async Task<int> Create(Community community)
        {
                _context.Communities.Add(community);
                return community.id;

        }



     
        public void Save()
        {
          _context.SaveChanges();
        }
    }
}
