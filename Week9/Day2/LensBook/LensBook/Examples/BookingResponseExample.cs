using LensBook.Dto_s.BookingDto_s;
using Swashbuckle.AspNetCore.Filters;

namespace LensBook.Examples
{
    public class BookingResponseExample
        : IExamplesProvider<BookingResponseDto>
    {
        public BookingResponseDto GetExamples()
        {
            return new BookingResponseDto
            {
                BookingId = 15,
                CustomerId = 3,
                PhotographerId = 1,
                SessionTypeId = 2,
                StartTime = new DateTime(2026, 9, 20, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 20, 11, 0, 0),
                Status = "Scheduled",
                Notes = "Outdoor portrait session"
            };
        }
    }
}