// This interface defines the database operations for the Medication repository.
// It handles retrieving, creating, updating, deleting, and filtering medications.

using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public interface IMedicationRepository
    {
        // Returns a medication by its ID.
        Task<Medication?> GetByIdAsync(int id);

        // Returns medications belonging to a specific patient.
        Task<IEnumerable<Medication>> GetByPatientIdAsync(int patientId);

        // Returns medications belonging to the authenticated user.
        Task<IEnumerable<Medication>> GetByUserIdAsync(int userId);

        // Returns all medications.
        Task<IEnumerable<Medication>> GetAllAsync();

        // Creates a new medication.
        Task<Medication> AddAsync(Medication medication);

        // Updates an existing medication.
        Task UpdateAsync(Medication medication);

        // Deletes an existing medication.
        Task DeleteAsync(Medication medication);

        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(int userId);

        // Filters medications belonging to the authenticated user.
        Task<IEnumerable<Medication>> FilterByUserIdAsync(
                int userId,
                MedicationFilterDto filter);

        // Returns medications that were active during
        // the cardiac-event analysis date range.
        Task<IEnumerable<Medication>>GetHistoricalMedicationsAsync(
        int patientId,
        DateTime startDate,
        DateTime eventDate);
    }
}