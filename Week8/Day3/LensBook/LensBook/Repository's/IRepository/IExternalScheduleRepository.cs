using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface IExternalScheduleRepository
    {
        Task<bool> HasOverlappingScheduleAsync(
            int photographerId,
            DateTime startTime,
            DateTime endTime);
    }
}