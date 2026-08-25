using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Insurance
    {
        [Key]
        public int InsuranceId { get; set; }

        public int PatientId { get; set; }

        public string ProviderName { get; set; } = string.Empty;

        public string PolicyNumber { get; set; } = string.Empty;

        public string? PlanName { get; set; }

        public string? CoverageType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Property

        public Patient Patient { get; set; } = null!;
    }
}
