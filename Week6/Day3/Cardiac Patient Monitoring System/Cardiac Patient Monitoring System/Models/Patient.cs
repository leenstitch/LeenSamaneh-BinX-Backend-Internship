using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Cardiac_Patient_Monitoring_System.Models
{
    public class Patient
    {
        public enum Gender
        {
            Male,
            Female,
        }
        [Key]
        public int PatientId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Gender PatientGender { get; set; }

        public string PrimaryPhone { get; set; } = string.Empty;

        public string NationalId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;

        public ICollection<EmergencyContact> EmergencyContacts { get; set; }
            = new List<EmergencyContact>();

        public ICollection<Diagnosis> Diagnoses { get; set; }
            = new List<Diagnosis>();

        public ICollection<VitalSign> VitalSigns { get; set; }
            = new List<VitalSign>();

        public ICollection<Medication> Medications { get; set; }
            = new List<Medication>();

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public ICollection<Doctor> Doctors { get; set; }
           = new List<Doctor>();

        public ICollection<Insurance> Insurances { get; set; }
            = new List<Insurance>();

        public ICollection<Allergy> Allergies { get; set; }
            = new List<Allergy>();

        public ICollection<FamilyMedicalHistory> FamilyMedicalHistories { get; set; }
            = new List<FamilyMedicalHistory>();

        public ICollection<LabResult> LabResults { get; set; }
            = new List<LabResult>();

        public ICollection<MedicalProcedure> MedicalProcedures { get; set; }
            = new List<MedicalProcedure>();

        public ICollection<Hospitalization> Hospitalizations { get; set; }
            = new List<Hospitalization>();

        public EmergencyMedicalInformation? EmergencyMedicalInformation { get; set; }

        public ICollection<Reminder> Reminders { get; set; }
            = new List<Reminder>();

        public ICollection<CardiacEvent> CardiacEvents { get; set; }
            = new List<CardiacEvent>();
    }
}
