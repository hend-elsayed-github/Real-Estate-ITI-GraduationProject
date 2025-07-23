using Real_Estate_Project.Models;
using System;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;


namespace Real_Estatae_Project.Repositories
{
    public class RentRepositories: IRentRepositories
    {


        private readonly ProjectContext _context;
        public RentRepositories(ProjectContext _Context)
        {
            _context = _Context;
        }


        public async Task GenerateMonthlyRentsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var rentedUnits = await _context.Units
                .Where(u => u.renterId != null && u.type == "For Rent")
                .ToListAsync();

            foreach (var unit in rentedUnits)
            {
                bool alreadyExists = await _context.Rents.AnyAsync(r =>
                    r.unitId == unit.id &&
                    r.dueDate.Month == today.Month &&
                    r.dueDate.Year == today.Year
                );

                if (!alreadyExists)
                {
                    var rent = new Rent
                    {
                        unitId = unit.id,
                        Rentvalue = unit.price, 
                        dueDate = new DateOnly(today.Year, today.Month, 1),
                        IsPaid=false,

                    };

                    _context.Rents.Add(rent);
                }
            }

            await _context.SaveChangesAsync();
        }
        #region Renter

        public async Task<IEnumerable<Rent>> UnpaidRentsAsync(string renterid)
        {

            var UnpaidRents = await _context.Rents
              .Where(r =>  r.unit.renterId == renterid && !r.IsPaid) 
              .Include(r => r.unit)
             .OrderByDescending(r => r.dueDate)
             .ToListAsync();
            return UnpaidRents;
        }

        public async Task<IEnumerable<Rent>> HistoryRentsAsync(string renterid)
        {
            var UnpaidRents = await _context.Rents
            .Where(r => r.unit.renterId == renterid )
           .Include(r => r.unit)
          .OrderByDescending(r => r.dueDate)
           .ToListAsync();
            return UnpaidRents;
  
        }
        #endregion

        #region Owner  
       



        public async Task<IEnumerable<Rent>> MonthRentsAsync(string ownerId, int month, int year)
        {
            return await _context.Rents
                .Include(r => r.unit)
                .Where(r =>
                r.unit.ownerId == ownerId &&
                    r.dueDate.Month == month &&
                    r.dueDate.Year == year
                )
                .ToListAsync();

        }
        #endregion


        public async Task<Rent?> GetRentByIdAsync(int rentId, string? renterId)
        {

            if (!string.IsNullOrEmpty(renterId))
            {
                return await _context.Rents
              
               .Where(r => r.id == rentId && !r.IsPaid)
                .Include(r => r.unit)
                .ThenInclude(u => u.owner)
               .FirstOrDefaultAsync();
            }
                return await _context.Rents
                .Where(r => r.id == rentId && !r.IsPaid ).FirstOrDefaultAsync();

        }



        public async Task UpdateRentAsync(int rentId)
        {
            var rent = await GetRentByIdAsync(rentId,null);
            if(rent != null){
                rent.IsPaid = true;
                await _context.SaveChangesAsync();

            }
        }

    }


}

