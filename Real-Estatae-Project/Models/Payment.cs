using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Payment
    {
        public int id { get; set; }
        public double  amount {  get; set; }
        
        public string paymentType { get; set; }

        //payment-paymentMethod relationship 1-m(payment)
        [ForeignKey("paymentMethod")]
        public int paymentMethodId { get; set; }
        public virtual  PaymentMethod paymentMethod { get; set; }
        public DateOnly PaymentDate {  get; set; }
        //bankAccount-payment 1-m(payment)
        [ForeignKey("bankAccount")]
        public int bankAccountId { get; set; }
        public virtual BankAccount bankAccount { get; set; }

        // user 1-m
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }

        // rent-payment 1-1

       

        public int RentId { get; set; }

      
        public virtual Rent Rent { get; set; }
    }
}
