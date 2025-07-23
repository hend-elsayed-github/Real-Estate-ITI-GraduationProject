using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Real_Estate_Project.Models;
using Stripe;

namespace Real_Estatae_Project.Repositories
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly ProjectContext _context;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(ProjectContext _Context, ILogger<PaymentRepository> logger)
        {
            _context = _Context;
            _logger = logger;
        }
      


        public async Task<int?> createPayment(Payment payment)
        {
            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                return payment.Id; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating payment");

                return null; 
            }
        }

       
    }
}
