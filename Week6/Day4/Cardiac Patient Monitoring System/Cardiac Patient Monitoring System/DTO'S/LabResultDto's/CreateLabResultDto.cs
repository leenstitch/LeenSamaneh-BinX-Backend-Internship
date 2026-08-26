using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s
{
    public class CreateLabResultDto
    {
      //  [Required]
       // public int PatientId { get; set; }

        [Required]
        public string TestName { get; set; } = string.Empty;

        [Required]
        public string Result { get; set; } = string.Empty;

        public string? Unit { get; set; }

        public string? ReferenceRange { get; set; }

        [Required]
        public DateTime TestDate { get; set; }

        public string? LaboratoryName { get; set; }

        public string? Notes { get; set; }
    }
}