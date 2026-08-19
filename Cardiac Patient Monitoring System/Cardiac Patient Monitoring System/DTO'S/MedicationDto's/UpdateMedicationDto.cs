using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s
{
    public class UpdateMedicationDto
    {
        [MinLength(2)]
        public string? PrescribedByDoctorName { get; set; }
        [MinLength(2)]
        public string? PrescribedBySpecialization { get; set; }
        [MinLength(2)]
        public string? Name { get; set; }
        [MinLength(1)]
        public string? Dosage { get; set; }
        [MinLength(1)]
        public string? Frequency { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}
