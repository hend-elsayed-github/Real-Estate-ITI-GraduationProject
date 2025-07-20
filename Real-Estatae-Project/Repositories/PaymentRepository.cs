using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly ProjectContext _context;
        public PaymentRepository(ProjectContext _Context)
        {
            _context = _Context;
        }


    }
}
