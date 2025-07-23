using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public interface INotificationRepository:IRepository<Notification>
    {

        Task AddAsync(Notification notification);
        Task<List<Notification>> GetUserNotifications(string userId);
        Task MarkAsRead(int notificationId);
    }
}
