using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s
{
    public class MedicationFilterDto
    {
        public string? Name { get; set; }

        public string? Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}