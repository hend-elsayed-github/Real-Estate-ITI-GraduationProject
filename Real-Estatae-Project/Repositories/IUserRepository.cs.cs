using Real_Estatae_Project.DTO.Unit;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IUserRepository :IRepository<ApplicationUser>
    {
        Task<int?> GetCommunityId(string userId,string role );
        Task<List<Unit>> getUnitBySSN(RenterSSNDTO renterSSN);

        Task setRenterCommunity(string renterId, Unit renterUnit);
        Task setRenterUnit(string renterId, int renterUnitId);


    }
}
