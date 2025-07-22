using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface IPaymentRepository: IRepository<Payment>
    {
        public Task<int?>createPayment (Payment payment);

    }
}
