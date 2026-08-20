// This interface defines the operations provided by the Patient service.
// It handles patient profile management, health summaries, and health status.

using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.Summary;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IPatientService
    {
        // Returns the profile of the authenticated patient.
        Task<PatientResponseDto?> GetMyProfileAsync(int userId);

        // Updates the profile of the authenticated patient.
        Task<PatientResponseDto?> UpdateMyProfileAsync(
            int userId,
            UpdatePatientDto dto);

        // Returns all patients.
        Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();

        // Deletes a patient by ID.
        Task<bool> DeletePatientAsync(int patientId);

        // Returns the health summary of the authenticated patient.
        Task<PatientHealthSummaryDto?>
            GetMyHealthSummaryAsync(int userId);

        // Returns the health summary of a specific patient.
        Task<PatientHealthSummaryDto?>
            GetHealthSummaryAsync(int patientId);

        // Returns the current health status of the authenticated patient.
        Task<PatientHealthStatusDto?>
            GetMyHealthStatusAsync(int userId);

        // Returns the current health status of a specific patient.
        Task<PatientHealthStatusDto?>
            GetHealthStatusAsync(int patientId);
    }
}