using System.ComponentModel.DataAnnotations;
using Cardiac_Patient_Monitoring_System.DTO_S.AllergyDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.FamilyHistoryDto_s;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s
{
    public class CreateAppointmentWithMedicalIntakeDto
    {
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string? Notes { get; set; }

        // New medical information
        public List<CreateAllergyDto> NewAllergies { get; set; } = new();

        public List<CreateFamilyHistoryDto> NewFamilyHistory { get; set; } = new();
    }
}
