using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Repositories
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly ProjectContext _context;
        public NotificationRepository(ProjectContext _Context)
        {
            _context = _Context;
        }

        public Task AddAsync(Notification notification)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetUserNotifications(string userId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsRead(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
