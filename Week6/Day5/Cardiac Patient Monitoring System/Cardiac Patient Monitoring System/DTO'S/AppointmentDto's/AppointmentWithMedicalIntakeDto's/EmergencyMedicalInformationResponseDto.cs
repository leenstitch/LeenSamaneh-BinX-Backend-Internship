namespace Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s.AppointmentWithMedicalIntakeDto_s
{
    public class EmergencyMedicalInformationResponseDto
    {
        public int EmergencyMedicalInformationId { get; set; }

        public string? BloodType { get; set; }

        public string? PreferredHospital { get; set; }

        public string? SpecialInstructions { get; set; }

        public string? EmergencyNotes { get; set; }
    }
}
