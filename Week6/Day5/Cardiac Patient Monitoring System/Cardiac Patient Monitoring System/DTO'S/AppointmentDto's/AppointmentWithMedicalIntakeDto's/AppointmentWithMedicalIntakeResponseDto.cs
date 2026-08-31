using Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s
{
    public class AppointmentWithMedicalIntakeResponseDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int ? DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string? Reason { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public List<AllergyResponseDto>? Allergies { get; set; } = new();

        public List<FamilyHistoryResponseDto>? FamilyHistory { get; set; } = new();

        public List<MedicationResponseDto>? Medications { get; set; } = new();

        public List<DiagnosisResponseDto>? Diagnoses { get; set; } = new();

        public EmergencyMedicalInformationResponseDto? EmergencyMedicalInformation { get; set; }
    }
}
