namespace LensBook.Dto_s.RegisterCustomerDto_s
{
    public class RegisterCustomerDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
    }
}
