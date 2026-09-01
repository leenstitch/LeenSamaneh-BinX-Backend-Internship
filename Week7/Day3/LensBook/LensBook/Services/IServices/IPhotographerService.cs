using LensBook.Dto_s.BookingDto_s;
using LensBook.Dto_s.PhotographerDto_s;

namespace LensBook.Services.IServices
{
    public interface IPhotographerService
    {
        Task<PhotographerResponseDto> CreateAsync(
           CreatePhotographerDto dto);


        Task<IEnumerable<BookingResponseDto>>
         GetMyBookingsAsync(
             int userId);
    }
}
