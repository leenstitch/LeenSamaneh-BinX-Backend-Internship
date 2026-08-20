namespace Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s
{
    public class PatientHealthSummaryDto
    {
        public PatientSummaryDto Patient { get; set; } = null!;

        public VitalSignSummaryDto? LatestVitalSigns { get; set; }

        public IEnumerable<MedicationSummaryDto> ActiveMedications { get; set; }
            = new List<MedicationSummaryDto>();

        public IEnumerable<DiagnosisSummaryDto> RecentDiagnoses { get; set; }
            = new List<DiagnosisSummaryDto>();

        public AppointmentSummaryDto? UpcomingAppointment { get; set; }
    }

    public class PatientSummaryDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string PrimaryPhone { get; set; } = string.Empty;
    }

    public class VitalSignSummaryDto
    {
        public int HeartRate { get; set; }

        public int SystolicPressure { get; set; }

        public int DiastolicPressure { get; set; }

        public decimal OxygenSaturation { get; set; }

        public decimal Temperature { get; set; }

        public DateTime MeasuredAt { get; set; }

        public string? Notes { get; set; }
    }

    public class MedicationSummaryDto
    {
        public int MedicationId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public string Frequency { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class DiagnosisSummaryDto
    {
        public int DiagnosisId { get; set; }

        public string DiagnosisName { get; set; } = string.Empty;

        public DateTime DiagnosedAt { get; set; }

        public string? DiagnosedByName { get; set; }

        public string? DiagnosedBySpecialization { get; set; }

        public string? Notes { get; set; }
    }

    public class AppointmentSummaryDto
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}