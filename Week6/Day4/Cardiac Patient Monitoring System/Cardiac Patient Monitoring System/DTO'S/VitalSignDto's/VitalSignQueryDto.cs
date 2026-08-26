// Defines the query parameters sent by the client
// for pagination, filtering, and sorting.
namespace Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s
{
    public class VitalSignQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? PatientName { get; set; }
        public string? Gender { get; set; }

        public string? Sort { get; set; }
    }
}
