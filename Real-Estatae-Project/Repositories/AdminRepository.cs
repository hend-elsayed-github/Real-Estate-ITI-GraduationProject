using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Admin;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ProjectContext Context;
        private readonly UserManager<ApplicationUser> userManager;
        public AdminRepository(ProjectContext _Context, UserManager<ApplicationUser> _userManager) 
        {
            Context = _Context;
            userManager = _userManager;
        }


        #region Get all Users
        public async Task<List<UserDTO>> GetAll()
        {
            var users = await userManager.Users.ToListAsync();
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                string role = roles.FirstOrDefault() ?? "None";

                int ownerUnits = await Context.Units.CountAsync(u => !u.isDeleted && u.ownerId == user.Id);
                int ownerAds = await Context.Addvertisements.CountAsync(ad => !ad.isDeleted && ad.userId == user.Id);

                string communityName = "";

                if (role == "Owner")
                {
                    communityName = await Context.Communities
                        .Where(c => c.ownerId == user.Id)
                        .Select(c => c.name)
                        .FirstOrDefaultAsync();
                }
                else if (role == "Renter")
                {
                    communityName = await Context.Users
                        .Where(u => u.Id == user.Id)
                        .Select(u => u.RenterCommunity.name)
                        .FirstOrDefaultAsync();
                }

                userDtos.Add(new UserDTO
                {
                    firstName = user.firstName,
                    lastName = user.lastName,
                    userName = user.UserName,
                    email = user.Email,
                    role = role,
                    communityName = communityName,
                    unitCount = ownerUnits,
                    adCount = ownerAds
                });
            }

            return userDtos;
        }

        #endregion

        #region Get all Owners
        public async Task<List<OwnerDTO>> GetAllOwners()
        {
            var owners = await userManager.GetUsersInRoleAsync("Owner");
            var ownerDtos = new List<OwnerDTO>();

            foreach (var owner in owners)
            {

                int ownerUnits = await Context.Units.CountAsync(u => !u.isDeleted && u.ownerId == owner.Id);
                int ownerAds = await Context.Addvertisements.CountAsync(ad => !ad.isDeleted && ad.userId == owner.Id);

                var communityName = await Context.Communities
                        .Where(c => c.ownerId == owner.Id)
                        .Select(c => c.name)
                        .FirstOrDefaultAsync();

                ownerDtos.Add(new OwnerDTO
                {
                    Id=owner.Id,
                    firstName = owner.firstName,
                    lastName = owner.lastName,
                    userName = owner.UserName,
                    email = owner.Email,
                    role = "Owner",
                    communityName = communityName,
                    unitCount = ownerUnits,
                    adCount = ownerAds
                });
            }
            return ownerDtos;
        }
        #endregion

        #region Get all Renters
        public async Task<List<RenterDTO>> GetAllRenters()
        {
            var renters = await userManager.GetUsersInRoleAsync("Renter");
            var renterDtos = new List<RenterDTO>();

            foreach (var renter in renters)
            {


                var communityName = await Context.Users
                         .Where(u => u.Id == renter.Id)
                         .Select(u => u.RenterCommunity.name)
                         .FirstOrDefaultAsync();

                renterDtos.Add(new RenterDTO
                {
                    firstName = renter.firstName,
                    lastName = renter.lastName,
                    userName = renter.UserName,
                    email = renter.Email,
                    role = "Renter",
                    communityName = communityName?? "Not Yet",
                    
                });
            }
            return renterDtos;
        }
        #endregion

        #region general numbers
        public async Task<object> GeneralNumbers()
        {
            int userCount = await userManager.Users.CountAsync(); // Includes admin

            // Get role-based user counts
            int ownerCount = (await userManager.GetUsersInRoleAsync("Owner")).Count;
            int renterCount = (await userManager.GetUsersInRoleAsync("Renter")).Count;

            // Unit counts
            int unitCount = await Context.Units.CountAsync(u=>u.isDeleted==false);
            int emptyUnitCount = await Context.Units.CountAsync(u => u.isDeleted == false && u.status == "empty");
            int busyUnitCount = unitCount - emptyUnitCount;

            int busyForRentUnitCount = await Context.Units.CountAsync(u => u.isDeleted == false && u.type == "For Rent" && u.status == "busy");
            int busyForSellUnitCount = busyUnitCount - busyForRentUnitCount;

            int emptyForRentUnitCount = await Context.Units.CountAsync(u => u.isDeleted == false && u.type == "For Rent" && u.status == "empty");
            int emptyForSellUnitCount = emptyUnitCount - emptyForRentUnitCount;

            // Other counts
            int communityCount = await Context.Communities.CountAsync();
            int adCount = await Context.Addvertisements.CountAsync(ad=>ad.isDeleted==false);

            // Profit
            double totalRent = await Context.Rents.Where(r => r.IsPaid == true).SumAsync(r => r.Rentvalue);

            decimal totalProfit =(decimal) totalRent * 0.1m;
            return new
            {
                userCount,
                ownerCount,
                renterCount,
                unitCount,
                emptyUnitCount,
                emptyForRentUnitCount,
                emptyForSellUnitCount,
                busyUnitCount,
                busyForRentUnitCount,
                busyForSellUnitCount,
                communityCount,
                adCount,
                totalProfit
            };
        }

        #endregion

        

    }
}
