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

        //bankAccount-payment 1-m(payment)
        [ForeignKey("bankAccount ")]
        public int bankAccountId { get; set; }
        public virtual BankAccount bankAccount { get; set; }

        // user 1-m
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }

        // bill-payment 1-1

        [ForeignKey("billParent")]
        public int billId { get; set; }
        public virtual BillParent billParent { get; set; }
    }
}
