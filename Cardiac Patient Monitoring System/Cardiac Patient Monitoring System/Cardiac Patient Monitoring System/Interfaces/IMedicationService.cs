// This interface defines the operations provided by the Medication service.
// It handles retrieving, creating, filtering, updating, and deleting medications.

using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IMedicationService
    {
        // Returns a medication by its ID.
        Task<MedicationResponseDto?> GetByIdAsync(int id);

        // Returns medications belonging to a specific patient.
        Task<IEnumerable<MedicationResponseDto>>
            GetByPatientIdAsync(int patientId);

        // Returns medications belonging to the authenticated patient.
        Task<IEnumerable<MedicationResponseDto>>
            GetMyMedicationsAsync(int userId);

        // Returns all medications.
        Task<IEnumerable<MedicationResponseDto>>
            GetAllAsync();

        // Creates a medication for the patient linked to the authenticated user.
        Task<MedicationResponseDto?> CreateAsync(
            int userId,
            CreateMedicationDto dto);

        // Updates an existing medication.
        Task<bool>
            UpdateAsync(
                int id,
                UpdateMedicationDto dto);

        // Deletes an existing medication.
        Task<bool>
            DeleteAsync(int id);

        // Filters medications belonging to the authenticated patient.
        Task<IEnumerable<MedicationResponseDto>>
            FilterMyMedicationsAsync(
                int userId,
                MedicationFilterDto filter);
    }
}