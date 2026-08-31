namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s
{
    public class DiagnosisResponseDto
    {
        public int DiagnosisId { get; set; }

        public string ?DiagnosisName { get; set; } = string.Empty;

        public DateTime? DiagnosedAt { get; set; }

        public string? DiagnosedByName { get; set; }

        public string? DiagnosedBySpecialization { get; set; }
       public DateTime? ConditionStartDate { get; set; }
        public string? Notes { get; set; }
    }
}
