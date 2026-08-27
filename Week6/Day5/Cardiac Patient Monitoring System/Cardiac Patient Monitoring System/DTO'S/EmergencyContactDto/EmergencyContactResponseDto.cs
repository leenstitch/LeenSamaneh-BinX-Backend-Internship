namespace Cardiac_Patient_Monitoring_System.DTO_S.EmergencyContactDto_s
{
    public class EmergencyContactResponseDto
    {
        public int EmergencyContactId { get; set; }

        public int PatientId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Relation { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }

        public string? Email { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}