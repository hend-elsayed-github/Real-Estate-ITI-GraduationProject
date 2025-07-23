using Real_Estatae_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public double  Amount {  get; set; }
        public string StripePaymentIntentId { get; set; }
        public string StripePaymentMethodId { get; set; }
        public DateOnly PaymentDate { get; set; } = DateOnly.FromDateTime( DateTime.UtcNow);

        public string paymentType { get; set; }
        public string? CardBrand { get; set; }
        public string? CardLast4 { get; set; }
        // user 1-m renter
        [ForeignKey("userId")]
        public string UserId { get; set; }
        public virtual ApplicationUser user { get; set; }
        // rent-payment 1-1
        public int RentId { get; set; }
        public virtual Rent Rent { get; set; }
    }
}
