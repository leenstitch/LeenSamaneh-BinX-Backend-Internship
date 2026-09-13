using LensBook.Dto_s.Notification_s;


namespace LensBook.Services.IServices
{
    public interface INotificationService
    {
        // Get all notifications for the current user
        Task<List<NotificationResponseDto>> GetMyNotificationsAsync();
        Task CreateAsync(
         int userId,
         string title,
         string message);
    }
}
