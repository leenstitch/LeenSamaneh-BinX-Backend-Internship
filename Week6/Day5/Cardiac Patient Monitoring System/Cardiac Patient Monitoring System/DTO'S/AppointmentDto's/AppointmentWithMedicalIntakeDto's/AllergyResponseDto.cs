namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s
{
    public class AllergyResponseDto
    {
        public int AllergyId { get; set; }

        public string? Name { get; set; } = string.Empty;

        public string? Reaction { get; set; }

        public string? Severity { get; set; }

        public DateTime? DiagnosedAt { get; set; }

        public string? Notes { get; set; }
    }
}
