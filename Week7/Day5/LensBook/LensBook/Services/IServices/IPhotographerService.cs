using LensBook.Dto_s.BookingDto_s;
using LensBook.Dto_s.PhotographerDto_s;

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
    }
}
