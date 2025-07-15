
using Microsoft.EntityFrameworkCore;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class UserRepository : IUserRepository

    {
        private readonly ProjectContext _Context;

        public UserRepository(ProjectContext Context)
        {
            _Context = Context;
        }
        public async Task<int?> GetCommunityId(string userId, string role)
        {

            if (role == "Renter")
            {
                var user = await _Context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

                return user?.communityId;
            }
            else if (role == "Owner")
            {
                return _Context.Communities
              .Where(c => c.ownerId == userId)
              .Select(c => c.id)
              .FirstOrDefault();
            }

            else
                return null;
        }
    }
}
