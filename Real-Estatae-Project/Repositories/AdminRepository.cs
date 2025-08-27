using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.DTO.Admin;
using Real_Estatae_Project.DTO.Reservation;
using Real_Estatae_Project.Models;
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


        #region Get active all Users
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
                    adCount = ownerAds,
                    isActive = user.isActive
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
                    Id = owner.Id,
                    firstName = owner.firstName,
                    lastName = owner.lastName,
                    userName = owner.UserName,
                    email = owner.Email,
                    role = "Owner",
                    communityName = communityName,
                    unitCount = ownerUnits,
                    adCount = ownerAds,
                    isActive = owner.isActive
                });
            }
            return ownerDtos;
        }
        #endregion

        #region Get all  Renters 
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
                    communityName = communityName ?? "Not Yet",
                    isActive = renter.isActive

                });
            }
            return renterDtos;
        }
        #endregion

        #region general numbers
        public async Task<object> GeneralNumbers()
        {
            int userCount = await userManager.Users.CountAsync(); // Includes admin and inActive users

            // Get role-based user counts
            int ownerCount = (await userManager.GetUsersInRoleAsync("Owner")).Count; //includes active and non active
            int renterCount = (await userManager.GetUsersInRoleAsync("Renter")).Count; //includes active and non active

            // Unit counts
            int unitCount = await Context.Units.CountAsync(u => u.isDeleted == false);
            int emptyUnitCount = await Context.Units.CountAsync(u => u.isDeleted == false && u.status == "empty");
            int busyUnitCount = unitCount - emptyUnitCount;

            int busyForRentUnitCount = await Context.Units.CountAsync(u => u.isDeleted == false && u.type == "For Rent" && u.status == "busy");
            int busyForSellUnitCount = busyUnitCount - busyForRentUnitCount;

            int emptyForRentUnitCount = await Context.Units.CountAsync(u => u.isDeleted == false && u.type == "For Rent" && u.status == "empty");
            int emptyForSellUnitCount = emptyUnitCount - emptyForRentUnitCount;

            // Other counts
            int communityCount = await Context.Communities.CountAsync();
            int adCount = await Context.Addvertisements.CountAsync(ad => ad.isDeleted == false);

            // Profit
            double totalRent = await Context.Rents.Where(r => r.IsPaid == true).SumAsync(r => r.Rentvalue);

            decimal totalProfit = (decimal)totalRent * 0.05m;
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


        //#region Delete the owner along with everything that belongs to them.
        //public async void  Delete(string ownerId)
        //{
        //    var ownerCommunity =await Context.Communities.Where(c => c.ownerId == ownerId).Select(c => c.id).FirstOrDefaultAsync();
        //    var ownerUnits =await Context.Units.Where(u => u.ownerId == ownerId).ToListAsync();
        //    List<ApplicationUser> renters = await userManager.Users.Where(u => u.communityId == ownerCommunity).ToListAsync();


        //}
        //#endregion

        #region profit per month
        public async Task<List<ProfitDTO>> GetProfitMonth()
        {
            var groupedData = await Context.Rents
                .Where(r => r.IsPaid)
                .GroupBy(r => new
                {
                    r.Payment.PaymentDate.Year,
                    r.Payment.PaymentDate.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Profit = g.Sum(r => (decimal)r.Rentvalue * 0.05m)
                })
                .ToListAsync();

            var profitDTOs = groupedData
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .Select(x => new ProfitDTO
                {
                    month = $"{x.Month:D2}/{x.Year}",
                    profit = x.Profit
                })
                .ToList();

            return profitDTOs;
        }

        #endregion

        #region transfer to new owner
        public async Task Transfer(string oldOwner, string newOwner)
        {
            using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                //new owner has by default a community so we will remove it to take this, our website allow one community for each owner
                var newCommunity = await Context.Communities.Where(c => c.ownerId == newOwner).FirstOrDefaultAsync();
                if (newCommunity != null)
                {
                    Context.Communities.Remove(newCommunity);
                }

                //old notifications, posts, reacts and comments will be deleted, but other things will be transfered to the nwe owner
                var oldNotifications = await Context.Notifications.Where(n => n.userId == oldOwner).ToListAsync();
                var oldReacts = await Context.Reacts.Where(r => r.UserId == oldOwner).ToListAsync();
                var oldComments = await Context.Comments.Where(c => c.userId == oldOwner).ToListAsync();
                var oldPosts = await Context.CommunityPosts.Where(p => p.userId == oldOwner).ToListAsync();
                //
                var oldUnits = await Context.Units.Where(u => u.ownerId == oldOwner).ToListAsync();
                var oldCommunity = await Context.Communities.Where(c => c.ownerId == oldOwner).FirstOrDefaultAsync(); //only one
                var oldAds = await Context.Addvertisements.Where(ad => ad.userId == oldOwner).ToListAsync();
                var oldAppointments = await Context.Appointments.Where(ap => ap.ownerId == oldOwner).ToListAsync();

                //delete
                Context.Notifications.RemoveRange(oldNotifications);
                Context.Reacts.RemoveRange(oldReacts);

                foreach (var comment in oldComments)
                {
                    comment.isDeleted = true;
                }

                foreach (var post in oldPosts)
                {
                    post.isDeleted = true;
                }

                //update the rest

                if (oldCommunity != null)
                {
                    oldCommunity.ownerId = newOwner;
                }

                foreach (var unit in oldUnits)
                {
                    unit.ownerId = newOwner;
                }

                foreach (var ad in oldAds)
                {
                    ad.userId = newOwner;
                }

                foreach (var ap in oldAppointments)
                {
                    ap.ownerId = newOwner;
                }

                //old owner still but inActive 
                var oldOwnerUser = await userManager.Users.FirstOrDefaultAsync(u => u.Id == oldOwner);
                oldOwnerUser.isActive = false;
                await userManager.UpdateAsync(oldOwnerUser);

                await Context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }


        #endregion

        #region ads vs Reservation
        public List<AdsVsReservationsDTO> GetMonthlyAdsVsReservations()
        {
            var data = Context.Addvertisements.GroupBy(a => a.publishDate.Month)
                .Select(g => new AdsVsReservationsDTO
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    AdsCount = g.Count(),
                    ReservationsCount = Context.Reservations.Count(r => r.reservationDate.Month == g.Key)
                }).ToList();
            return data;
        }

        #endregion

        #region profitperCommunity
        public async Task<List<ProfitPerCommunityDTO>> GetProfitPerCommunity()
        {
            var result = await Context.Rents
                .Where(r => r.IsPaid && r.unit != null && r.unit.community != null)
                .GroupBy(r => r.unit.community.name)
                .Select(g => new ProfitPerCommunityDTO
                {
                    CommunityName = g.Key,
                    Profit = g.Sum(r => (decimal)r.Rentvalue * 0.05m)
                }).ToListAsync();
            return result;
        }


        #endregion

        #region GetReseervation
        public List<AllReserDTO> GetReservation()
        {
            List<Reservation> reservations = Context.Reservations
                .Where(r => r.status == ReservationStatus.Completed || r.status == ReservationStatus.Cancelled)
                .Include(r => r.appointment)
                    .ThenInclude(app => app.advertisement)
                        .ThenInclude(ad => ad.unit)
                 .Include(r => r.appointment)
                 .ThenInclude(r => r.owner)

                .ToList();

            var result = reservations.Select(r => new AllReserDTO
            {
                id = r.id,
                appointmentId = r.appointment?.id ?? 0,
                email = r.email,
                name = r.name,
                phoneNumber = r.phoneNumber,
                reservationDate = r.reservationDate,
                Status = r.status.ToString() ?? "Unknown",
                Location =
                (r.appointment?.advertisement?.unit?.city ?? "N/A") + ", " +
                (r.appointment?.advertisement?.unit?.area ?? "N/A") + ", " +
                (r.appointment?.advertisement?.unit?.street ?? "N/A"),
                owner = (r.appointment?.owner?.firstName ?? "") + " " + (r.appointment?.owner?.lastName ?? "")
            }).ToList();

            return result;

        }
    
        #endregion

        #region get renters by community

        public async Task<List<RenterDTO>> GetRentersByCommunity(string communityName)
        {
            var renters = await userManager.Users
                .Where(u => u.RenterCommunity != null && u.RenterCommunity.name == communityName)
                .ToListAsync();
            var renterDtos = new List<RenterDTO>();
            foreach (var renter in renters)
            {
                renterDtos.Add(new RenterDTO
                {
                    firstName = renter.firstName,
                    lastName = renter.lastName,
                    userName = renter.UserName,
                    email = renter.Email,
                    role = "Renter",
                    communityName = communityName,
                    isActive = renter.isActive
                });
            }
            return renterDtos;
        }

        #endregion

        #region get renters who doesn't have a community yet
        public async Task<List<RenterDTO>> GetRentersWithNoCommunity()
        {
            var renters = await userManager.Users
                .Where(u => u.RenterCommunity == null)
                .ToListAsync();
            var renterDtos = new List<RenterDTO>();
            foreach (var renter in renters)
            {
                renterDtos.Add(new RenterDTO
                {
                    firstName = renter.firstName,
                    lastName = renter.lastName,
                    userName = renter.UserName,
                    email = renter.Email,
                    role = "Renter",
                    communityName = "Not Yet",
                    isActive = renter.isActive
                });
            }
            return renterDtos;
        }

        #endregion

    }
}
