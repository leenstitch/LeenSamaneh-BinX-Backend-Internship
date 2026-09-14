using LensBook.Dto_s.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace LensBook.Examples
{
    public class LoginExample : IExamplesProvider<LoginDto>
    {
        public LoginDto GetExamples()
        {
            return new LoginDto
            {
                Email = "leen@example.com",
                Password = "P@ssword123"
            };
        }
    }
}