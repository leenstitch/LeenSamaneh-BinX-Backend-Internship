namespace LensBook.Dto_s.PhotographerDto_s
{
    public class PhotographerResponseDto
    {
        public int PhotographerId { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Bio { get; set; }

       

        public string Role { get; set; } = "Photographer";
    }
}
