using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public int PatientId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? ClinicName { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Properties

        public Patient Patient { get; set; } = null!;

        public ICollection<Diagnosis> Diagnoses { get; set; }
            = new List<Diagnosis>();

        public ICollection<Medication> Medications { get; set; }
            = new List<Medication>();

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public ICollection<MedicalProcedure> MedicalProcedures { get; set; }
            = new List<MedicalProcedure>();

        public ICollection<CardiacEvent> CardiacEvents { get; set; }
            = new List<CardiacEvent>();
    }
}
