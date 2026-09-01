using LensBook.DATA;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        //create booking
        public async Task AddAsync(
            Booking booking)
        {
            await _context.Bookings.AddAsync(
                booking);
        }

        //save changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

       
    }
}