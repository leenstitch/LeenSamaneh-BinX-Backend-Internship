namespace Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s
{
    public class MedicalProcedureResponseDto
    {
        public int ProcedureId { get; set; }

        public int PatientId { get; set; }

        public string ProcedureName { get; set; } = string.Empty;

        public DateTime ProcedureDate { get; set; }

        public string? HospitalName { get; set; }

        public string? Reason { get; set; }

        public string? Outcome { get; set; }

        public string? Notes { get; set; }
        public int ? DoctorId { get; set; }
    }
}