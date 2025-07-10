using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class BankAccount
    {
        public int id { get; set; }

        public string bankName { get; set; }

        public string accountNumber { get; set; }

        public virtual List<Payment> Payments { get; set; }

        // user(owner-renter) 1-m
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}
