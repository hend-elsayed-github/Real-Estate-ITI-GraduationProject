using Real_Estate_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Real_Estatae_Project.Repositories
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly ProjectContext _context;
        public NotificationRepository(ProjectContext _Context)
        {
            _context = _Context;
        }
       
        public async Task AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetUserNotifications(string userId)
        {
            return await _context.Notifications
                    .Where(n => n.userId == userId)
                    .OrderByDescending(n => n.createAt)
                    .ToListAsync();
        }

        public async  Task MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
                return;

            notification.isRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
