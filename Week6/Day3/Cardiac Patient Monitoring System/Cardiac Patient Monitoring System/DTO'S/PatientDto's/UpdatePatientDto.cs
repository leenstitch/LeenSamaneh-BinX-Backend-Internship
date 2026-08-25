using System.ComponentModel.DataAnnotations;
using Cardiac_Patient_Monitoring_System.Models;
using static Cardiac_Patient_Monitoring_System.Models.Patient;

namespace Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s
{
    public class UpdatePatientDto
    {
        [MinLength(2)]
        public string? FirstName { get; set; } = string.Empty;
        [MinLength(2)]
        public string? LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public Patient.Gender? PatientGender { get; set; }
        [Phone]
        public string? PrimaryPhone { get; set; } = string.Empty;
    }
}
