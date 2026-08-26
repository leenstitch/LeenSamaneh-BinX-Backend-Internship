namespace Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s
{
    public class HospitalizationResponseDto
    {
        public int HospitalizationId { get; set; }

        public int PatientId { get; set; }

        public string HospitalName { get; set; } = string.Empty;

        public DateTime AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        public string? Reason { get; set; }

        public string? Diagnosis { get; set; }

        public string? Notes { get; set; }
    }
}