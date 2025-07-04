namespace Real_Estate_Project.Models
{
    public class PaymentMethod
    {
        public int id { get; set; }

        public string name { get; set; }

        // payment-paymentMethod  1-m(payment)
        public virtual List<Payment> Payments { get; set; }
    }
}
