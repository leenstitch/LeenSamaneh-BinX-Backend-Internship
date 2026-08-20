using System.ComponentModel.DataAnnotations;
using System.Reflection;

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
    }
}
