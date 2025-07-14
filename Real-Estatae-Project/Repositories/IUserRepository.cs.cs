using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IUserRepository :IRepository<ApplicationUser>
    {
        Task<int?> GetCommunityId(string userId,string role );

    }
}
