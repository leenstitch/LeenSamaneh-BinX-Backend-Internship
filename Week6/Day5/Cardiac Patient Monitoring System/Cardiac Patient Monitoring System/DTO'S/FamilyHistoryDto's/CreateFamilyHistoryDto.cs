using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.FamilyHistoryDto_s
{
    public class CreateFamilyHistoryDto
    {
        [Required]
        public string Relationship { get; set; } = string.Empty;

        [Required]
        public string Condition { get; set; } = string.Empty;

        public int? AgeAtDiagnosis { get; set; }

        public string? Notes { get; set; }
    }
}
