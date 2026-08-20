using System.ComponentModel.DataAnnotations;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s
{
    public class UpdateDiagnosisDto
    {

        [MinLength(2)]
        public string? DiagnosedByName { get; set; }
        [MinLength(2)]
        public string? DiagnosedBySpecialization { get; set; }
        [MinLength(2)]
        public string? DiagnosisName { get; set; }

        public DateTime? DiagnosedAt { get; set; }

        public string? Notes { get; set; }

        
    }
}