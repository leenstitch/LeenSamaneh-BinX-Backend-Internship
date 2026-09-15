using LensBook.Dto_s.BookingDto_s;
using Swashbuckle.AspNetCore.Filters;

namespace LensBook.Examples
{
    public class CreateBookingExample
        : IExamplesProvider<CreateBookingDto>
    {
        public CreateBookingDto GetExamples()
        {
            return new CreateBookingDto
            {
                PhotographerId = 1,
                SessionTypeId = 2,
                StartTime = new DateTime(2026, 9, 20, 10, 0, 0),
                Notes = "Outdoor portrait session"
            };
        }
    }
}