using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface INotificationRepository
    {
        // Get all notifications for a specific user
        Task<List<Notification>> GetByUserIdAsync(int userId);
        Task AddAsync(Notification notification);
      //  Task SaveChangesAsync();
    }
}
