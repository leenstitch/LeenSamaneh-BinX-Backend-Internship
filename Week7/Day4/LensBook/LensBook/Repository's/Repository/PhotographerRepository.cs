using LensBook.DATA;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Repositories
{
    public class PhotographerRepository : IPhotographerRepository
    {
        private readonly ApplicationDbContext _context;

        public PhotographerRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // get photographer by id 
        public async Task<Photographer?> GetByIdAsync(
            int photographerId)
        {
            return await _context.Photographers
                .Include(p => p.User)
                .FirstOrDefaultAsync(
                    p => p.PhotographerId == photographerId);
        }

        // get photographerId by user id
        public async Task<Photographer?> GetByUserIdAsync(
            int userId)
        {
            return await _context.Photographers
                .Include(p => p.User)
                .FirstOrDefaultAsync(
                    p => p.UserId == userId);
        }

        // create photographer
        public async Task AddAsync(
            Photographer photographer)
        {
            await _context.Photographers.AddAsync(
                photographer);
        }

        //save changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        //get all customers bookings for a specific photographer
        public async Task<IEnumerable<Booking>> GetMyBookingsAsync(
    int photographerId)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.SessionType)
                .Where(b => b.PhotographerId == photographerId)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }
    }
}