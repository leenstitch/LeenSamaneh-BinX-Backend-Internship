using LensBook.Dto_s.BookingDto_s;
using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface IBookingRepository
    {

        //create booking
        Task AddAsync(Booking booking);

        //save changes to the database
        Task SaveChangesAsync();

        // check if a photographer has overlapping bookings
        Task<bool> HasOverlappingBookingAsync(
    int photographerId,
    DateTime startTime,
    DateTime endTime);


        // get booking by ID
        Task<Booking?> GetByIdAsync(int bookingId);

       
    }
}