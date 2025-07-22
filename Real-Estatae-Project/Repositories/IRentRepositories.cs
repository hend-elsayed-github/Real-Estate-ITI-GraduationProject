using Microsoft.Extensions.Logging;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IRentRepositories : IRepository<Rent>
    {
        public  Task GenerateMonthlyRentsAsync();
        public  Task<IEnumerable<Rent>>UnpaidRentsAsync(string renterid);
        public  Task<IEnumerable<Rent>> HistoryRentsAsync(string renterid);

        public Task<IEnumerable<Rent>> MonthRentsAsync(string ownerid, int month, int year);

        public Task <Rent?> GetRentByIdAsync(int rentId, string? renterId=null);
        public Task  UpdateRentAsync(int rentId);
    }
}
