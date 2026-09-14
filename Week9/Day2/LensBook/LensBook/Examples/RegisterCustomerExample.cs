using LensBook.Dto_s.RegisterCustomerDto_s;
using Swashbuckle.AspNetCore.Filters;

namespace LensBook.Examples
{
    public class RegisterCustomerExample
        : IExamplesProvider<RegisterCustomerDto>
    {
        public RegisterCustomerDto GetExamples()
        {
            return new RegisterCustomerDto
            {
                FirstName = "Leen",
                LastName = "Samaneh",
                Email = "leen@example.com",
                Password = "P@ssword123",
                PhoneNumber = "0599123456"
            };
        }
    }
}