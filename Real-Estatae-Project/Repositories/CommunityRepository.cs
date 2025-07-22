using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Real_Estatae_Project.DTO.Community;
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

        public int GetCommunityId(string ownerId)
        {
            return _context.Communities
                .Where(c => c.ownerId == ownerId)
                .Select(c => c.id)
                .FirstOrDefault();
        }


        public  bool  Update(string ownerId, CommunityInfoDTO newName)
        {
           Community ownerComm = _context.Communities
                .FirstOrDefault(c => c.ownerId == ownerId);

            if (ownerComm != null)
            {
                ownerComm.name = newName.Name;
                
                Save();

                return true;
            }
            return false;
        }

        public string GetName(int commId)
        {
            return  _context.Communities
                .Where(c => c.id == commId)
                .Select(c => c.name)
                .FirstOrDefault();

        }


        public void Save()
        {
          _context.SaveChanges();
        }
    }
}
