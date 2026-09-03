using System.ComponentModel.DataAnnotations;

namespace LensBook.Dto_s.SessionType
{
    public class CreateSessionTypeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int DurationInMinutes { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;
    }
}