using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s
{
    public class DiagnosisFilterDto
    {
        [MinLength(2)]
        public string? PatientName { get; set; }
        [Range(0, 120)]
        public int? Age { get; set; }

        public string? Gender { get; set; }

        public string? NationalId { get; set; }
        [MinLength(2)]
        public string? DiagnosisName { get; set; }
       
        public string? Status { get; set; }
    }
}