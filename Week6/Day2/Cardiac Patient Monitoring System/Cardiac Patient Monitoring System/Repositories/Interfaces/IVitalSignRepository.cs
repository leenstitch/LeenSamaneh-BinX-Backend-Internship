// This interface defines the database operations for the VitalSign repository.
// It handles retrieving, creating, updating, deleting, filtering,
// and comparing patient vital-sign records.

using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IVitalSignRepository
    {
        // Returns a vital-sign record by its ID.
        Task<VitalSign?> GetByIdAsync(int id);

        // Returns vital-sign records belonging to a specific patient.
        Task<IEnumerable<VitalSign>>
            GetByPatientIdAsync(int patientId);

        // Returns all vital-sign records.
        Task<IEnumerable<VitalSign>>
            GetAllAsync();

        // Creates a new vital-sign record.
        Task<VitalSign>
            AddAsync(VitalSign vitalSign);

        // Updates an existing vital-sign record.
        Task UpdateAsync(VitalSign vitalSign);

        // Deletes an existing vital-sign record.
        Task DeleteAsync(VitalSign vitalSign);

        // Filters vital-sign records using patient information.
        Task<IEnumerable<VitalSign>> FilterAsync(
            string? patientName,
            int? age,
            string? gender,
            string? nationalId);

        // Returns vital-sign records belonging to the authenticated user.
        Task<IEnumerable<VitalSign>>
            GetByUserIdAsync(int userId);

        // Returns the two latest vital-sign records for a user.
        Task<List<VitalSign>> GetLatestTwoByUserIdAsync(
            int userId);

        // Returns the latest vital-sign record for a user on a specific date.
        Task<VitalSign?> GetLatestByUserIdAndDateAsync(
            int userId,
            DateTime date);

        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(
            int userId);
    }
}