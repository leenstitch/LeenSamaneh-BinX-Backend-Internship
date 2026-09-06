using System.Security.Claims;
using LensBook.Dto_s.Notification_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services.IServices;

namespace LensBook.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationService(
            INotificationRepository notificationRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<NotificationResponseDto>> GetMyNotificationsAsync()
        {
            // Get current authenticated user
            var user =
                _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }

            // Get UserId from JWT
            var userIdClaim =
              user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null ||
                !int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                throw new UnauthorizedAccessException(
                    "User information was not found.");
            }

            // Get notifications for current user
            var notifications =
                await _notificationRepository
                    .GetByUserIdAsync(userId);

            // Convert Entity → DTO
            return notifications
                .Select(n => new NotificationResponseDto
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToList();
        }

        public async Task CreateAsync(
           int userId,
           string title,
           string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository
                .AddAsync(notification);


            //await _notificationRepository
            //    .SaveChangesAsync();
        }
    }
}