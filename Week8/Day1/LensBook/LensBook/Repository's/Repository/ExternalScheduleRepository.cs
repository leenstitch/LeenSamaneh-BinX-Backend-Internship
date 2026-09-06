using LensBook.DATA;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Repositories
{
    public class ExternalScheduleRepository
        : IExternalScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public ExternalScheduleRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasOverlappingScheduleAsync(
            int photographerId,
            DateTime startTime,
            DateTime endTime)
        {
            return await _context.ExternalSchedules
                .AnyAsync(e =>
                    e.PhotographerId == photographerId &&
                    e.StartTime < endTime &&
                    e.EndTime > startTime);
        }
    }
}