using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface IBookingRepository
    {

        //create booking
        Task AddAsync(Booking booking);

        //save changes to the database
        Task SaveChangesAsync();

        
    }
}
