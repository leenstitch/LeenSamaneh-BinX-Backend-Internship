using LensBook.Dto_s.BookingDto_s;
using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface IPhotographerRepository
    {

        // Get photographer by id
        Task<Photographer?> GetByIdAsync(int photographerId);

        // Get photographerId by user id
        Task<Photographer?> GetByUserIdAsync(int userId);
        
        // Add a new photographer
        Task AddAsync(Photographer photographer);

        // Get all bookings for a photographer
        Task<IEnumerable<Booking>> GetMyBookingsAsync(int photographerId);

        //save changes to the database
        Task SaveChangesAsync();

        // Get all bookings for a photographer using projection
        Task<IEnumerable<BookingResponseDto>> GetMyBookingsAsyncByUsingprojection(
            int photographerId);

        // Get photographer details with collections
        Task<Photographer?> GetDetailsWithCollectionsAsync(
    int photographerId);
    }

}
