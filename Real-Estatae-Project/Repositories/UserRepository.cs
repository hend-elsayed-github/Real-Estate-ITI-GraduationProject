

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO;

using Real_Estatae_Project.DTO.Unit;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class UserRepository : IUserRepository

    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ProjectContext _Context;

        public UserRepository(UserManager<ApplicationUser> userManager, ProjectContext Context)
        {
            _Context = Context;
            _userManager = userManager;
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



        public async Task< List<Unit>> getUnitBySSN(RenterSSNDTO renterSSN)

        {
            List<Unit> units = _Context.Units.Where(u => u.status == "busy" && u.isDeleted == false && u.renterSSN == renterSSN.SSN)
                .ToList();
            if (units == null)
            {
                return null;
            }

            return  units;

        }


        public async Task setRenterCommunity(string renterId, Unit renterUnit)
        {
            ApplicationUser renter = await _Context.Users
                .FirstOrDefaultAsync(u => u.Id == renterId);
            if (renter != null)
            {
                renter.communityId = renterUnit.communityId;
                _Context.SaveChangesAsync();
            }

        }

        public async Task setRenterUnit(string renterId, int renterUnitId)
        {
            Unit unit = await _Context.Units
                .FirstOrDefaultAsync(u => u.id == renterUnitId && u.isDeleted == false);

            if (unit != null)
            {
                unit.renterId = renterId;
                await _Context.SaveChangesAsync();
            }
        }



        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await _Context.Users.FindAsync(userId);
        }

        public async Task Update(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }



        public async Task<List<string>> GetUserIdsInCommunity(int communityId)
        {
            return await _Context.Users
                .Where(u => u.communityId == communityId)
                .Select(u => u.Id)
                .ToListAsync();
        }


        ///////////////////////////////////
        public async Task<List<UserCommunityDTO>> GetTopActiveUsersByCommunityAsync(string userId)
        {
            // Get the communityId (as user or owner)
            int? communityId = await _Context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.communityId)
                .FirstOrDefaultAsync();

            if (communityId == null)
            {
                communityId = await _Context.Communities
                    .Where(c => c.ownerId == userId)
                    .Select(c => c.id)
                    .FirstOrDefaultAsync();
            }

            if (communityId == null)
                return new List<UserCommunityDTO>();

            // Get users in the same community
            var usersInCommunity = await _Context.Users
                .Where(u => u.communityId == communityId)
                .Select(u => new
                {
                    u.Id,
                    u.firstName,
                    u.lastName,
                    u.Email,
                    u.image
                })
                .ToListAsync();

            var userIds = usersInCommunity.Select(u => u.Id).ToList();

            // Count posts per user
            var postCounts = await _Context.CommunityPosts
                .Where(p => userIds.Contains(p.userId) && p.communityId == communityId && !p.isDeleted)
                .GroupBy(p => p.userId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();

            // Count comments per user
            var commentCounts = await _Context.Comments
                .Where(c => userIds.Contains(c.userId) && c.communityPost.communityId == communityId && !c.isDeleted)
                .GroupBy(c => c.userId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();

            // Count reacts per user
            var reactCounts = await _Context.Reacts
                .Where(r => userIds.Contains(r.UserId) && r.Post.communityId == communityId)
                .GroupBy(r => r.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToListAsync();

            // Merge all counts
            var topUsers = usersInCommunity
                .Select(u =>
                {
                    int postCount = postCounts.FirstOrDefault(p => p.UserId == u.Id)?.Count ?? 0;
                    int commentCount = commentCounts.FirstOrDefault(c => c.UserId == u.Id)?.Count ?? 0;
                    int reactCount = reactCounts.FirstOrDefault(r => r.UserId == u.Id)?.Count ?? 0;

                    return new UserCommunityDTO
                    {
                        Name = u.firstName + " " + u.lastName,
                        Email = u.Email,
                        Image = u.image,
                        PostCount = postCount,
                        CommentCount = commentCount,
                        ReactCount = reactCount
                    };
                })
                .OrderByDescending(u => u.PostCount + u.CommentCount + u.ReactCount)
                .Take(5)
                .ToList();

            return topUsers;
        }

        public async Task<UserCommunityDTO> GetUserCommunity(string userId)

        {

            UserCommunityDTO user = await _Context.Users.Where(u => u.Id == userId)

                .Select(user =>

                new UserCommunityDTO

                {

                    Name = user.firstName + ' ' + user.lastName,

                    Email = user.Email,

                    Image = user.image,

                    UserName = user.UserName,

                    PostCount = _Context.CommunityPosts.Count(p => p.userId == userId && !p.isDeleted),

                    CommentCount = _Context.Comments.Count(c => c.userId == userId && !c.isDeleted),

                    ReactCount = _Context.Reacts.Count(r => r.UserId == userId)

                }).FirstOrDefaultAsync();

            return user;

        }



    }
}

