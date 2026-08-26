using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s
{
    public class CreateMedicationDto
    {

        [Required]
        [MinLength(2)]
        public string? PrescribedByDoctorName { get; set; }
        [Required]
        [MinLength(2)]
        public string? PrescribedBySpecialization { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string Frequency { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}