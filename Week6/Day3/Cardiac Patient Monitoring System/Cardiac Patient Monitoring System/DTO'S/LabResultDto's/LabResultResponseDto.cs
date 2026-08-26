namespace Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s
{
    public class LabResultResponseDto
    {
        public int LabResultId { get; set; }

        public int PatientId { get; set; }

        public string TestName { get; set; } = string.Empty;

        public string Result { get; set; } = string.Empty;

        public string? Unit { get; set; }

        public string? ReferenceRange { get; set; }

        public DateTime TestDate { get; set; }

        public string? LaboratoryName { get; set; }

        public string? Notes { get; set; }
    }
}