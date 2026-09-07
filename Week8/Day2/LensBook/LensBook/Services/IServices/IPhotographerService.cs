using LensBook.Dto_s.BookingDto_s;
using LensBook.Dto_s.PhotographerDto_s;
using LensBook.Models;

namespace LensBook.Services.IServices
{
    public interface IPhotographerService
    {
        //create photographer
        Task<PhotographerResponseDto> CreateAsync(
           CreatePhotographerDto dto);


        //get session types by photographer id
        Task<IEnumerable<BookingResponseDto>>
         GetMyBookingsAsync(
             int userId);

        // Get bookings for the authenticated photographer
        Task<IEnumerable<BookingResponseDto>>
            GetMyBookingsAsyncByUsingProjection(int userId);

        Task<PhotographerDetailsDto?> GetDetailsWithCollectionsAsync(
         int photographerId);
    }
}
