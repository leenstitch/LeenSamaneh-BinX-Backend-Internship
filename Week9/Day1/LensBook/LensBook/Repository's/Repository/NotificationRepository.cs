using LensBook.DATA;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all notifications for a specific user
        public async Task<List<Notification>> GetByUserIdAsync(
            int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // this method to add a new notification to the database
        public async Task AddAsync(
           Notification notification)
        {
            await _context.Notifications
                .AddAsync(notification);
        }

        //public async Task SaveChangesAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}
    }
}