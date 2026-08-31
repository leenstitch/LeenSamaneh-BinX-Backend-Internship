namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s
{
    public class FamilyHistoryResponseDto
    {
        public int FamilyHistoryId { get; set; }

        public string? Relationship { get; set; } = string.Empty;

        public string? Condition { get; set; } = string.Empty;

        public int? AgeAtDiagnosis { get; set; }

        public string? Notes { get; set; }
    }
}
