
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO;
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
    }
}
