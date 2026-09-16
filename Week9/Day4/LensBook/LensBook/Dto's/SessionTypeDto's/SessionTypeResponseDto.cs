namespace LensBook.Dto_s.SessionTypeDto_s
{
    public class SessionTypeResponseDto
    {
        public int SessionTypeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DurationInMinutes { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
