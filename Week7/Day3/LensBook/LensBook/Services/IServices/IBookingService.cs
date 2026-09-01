using LensBook.Dto_s.BookingDto_s;

namespace LensBook.Services.IServices
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateAsync(
           CreateBookingDto dto);

    }
}
