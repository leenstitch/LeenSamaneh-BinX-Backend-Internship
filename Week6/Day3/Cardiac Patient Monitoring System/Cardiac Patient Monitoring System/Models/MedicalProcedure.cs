using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class MedicalProcedure
    {
        [Key]
        public int ProcedureId { get; set; }

        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        public string ProcedureName { get; set; } = string.Empty;

        public DateTime ProcedureDate { get; set; }

        public string? HospitalName { get; set; }

        public string? Reason { get; set; }

        public string? Outcome { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Properties

        public Patient Patient { get; set; } = null!;

        public Doctor? Doctor { get; set; }
    }
}
