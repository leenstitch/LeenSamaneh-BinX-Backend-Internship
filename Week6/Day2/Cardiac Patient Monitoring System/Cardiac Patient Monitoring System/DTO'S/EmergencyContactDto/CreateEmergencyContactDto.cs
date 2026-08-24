using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.EmergencyContactDto_s
{
    public class CreateEmergencyContactDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [MinLength(2)]
        public string Relation { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public string? Notes { get; set; }
    }
}