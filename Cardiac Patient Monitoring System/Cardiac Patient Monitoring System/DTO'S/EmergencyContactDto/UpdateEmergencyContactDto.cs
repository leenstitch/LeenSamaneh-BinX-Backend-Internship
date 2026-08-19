using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.EmergencyContactDto_s
{
    public class UpdateEmergencyContactDto
    {
        [MinLength(2)]
        public string? Name { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [MinLength(2)]
        public string? Relation { get; set; }

        public bool? IsPrimary { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public string? Notes { get; set; }
    }
}