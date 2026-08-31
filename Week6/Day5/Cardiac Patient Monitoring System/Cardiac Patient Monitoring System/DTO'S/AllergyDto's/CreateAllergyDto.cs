using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AllergyDto_s
{
    public class CreateAllergyDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Reaction { get; set; }

        public string? Severity { get; set; }

        public DateTime? DiagnosedAt { get; set; }

        public string? Notes { get; set; }
    }
}
