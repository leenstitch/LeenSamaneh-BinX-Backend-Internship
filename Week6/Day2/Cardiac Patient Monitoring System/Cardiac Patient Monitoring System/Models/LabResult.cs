using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class LabResult
    {
        [Key]
        public int LabResultId { get; set; }

        public int PatientId { get; set; }

        public string TestName { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public string? Unit { get; set; }

        public string? ReferenceRange { get; set; }

        public DateTime TestDate { get; set; }

        public string? LaboratoryName { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property

        public Patient Patient { get; set; } = null!;
    }
}
