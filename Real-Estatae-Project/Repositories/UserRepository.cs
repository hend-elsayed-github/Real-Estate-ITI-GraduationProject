
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.DTO.Unit;
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

        ///////////////////////////////////
        public async Task<List<UserCommunityDTO>> GetTopActiveUsersByCommunityAsync(string userId)

        {

            int? communityId = await _Context.Users.Where(u => u.Id == userId).Select(c => c.communityId).FirstOrDefaultAsync();

            if (communityId == null)

                communityId = await _Context.Communities.Where(u => u.ownerId == userId).Select(c => c.id).FirstOrDefaultAsync();

            if (communityId == null)

                return new List<UserCommunityDTO>();

            var users = await _Context.Users

        .Select(user => new UserCommunityDTO

        {

            Name = user.firstName + " " + user.lastName,

            Email = user.Email,

            Image = user.image,

            PostCount = _Context.CommunityPosts

                            .Count(p => p.userId == user.Id && p.communityId == communityId && !p.isDeleted),

            CommentCount = _Context.Comments

                            .Count(c => c.userId == user.Id && c.communityPost.communityId == communityId && !c.isDeleted),

            ReactCount = _Context.Reacts

                            .Count(r => r.UserId == user.Id && r.Post.communityId == communityId)

        })

        .ToListAsync();

            var topUsers = users

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
