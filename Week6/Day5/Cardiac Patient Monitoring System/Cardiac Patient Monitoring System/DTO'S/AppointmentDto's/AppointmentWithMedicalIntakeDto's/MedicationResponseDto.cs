namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s
{
    public class MedicationResponseDto
    {
        public int MedicationId { get; set; }

        public string? Name { get; set; } = string.Empty;

        public string? Dosage { get; set; } = string.Empty;

        public string ?Frequency { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
      public string? PrescribedBySpecialization {  get; set; }
        public string? PrescribedByDoctorName { get; set; }
    }
}
