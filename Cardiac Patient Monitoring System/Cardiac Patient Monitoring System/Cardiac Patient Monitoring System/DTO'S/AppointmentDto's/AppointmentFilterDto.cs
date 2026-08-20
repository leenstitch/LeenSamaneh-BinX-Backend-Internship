using System.ComponentModel.DataAnnotations;

namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s
{
    public class AppointmentFilterDto
    {
        public DateTime? Date { get; set; }
        [Range(2000, 2100)]
        public int? Year { get; set; }
        [Range(1, 12)]
        public int? Month { get; set; }

        public string? Status { get; set; }
    }
}