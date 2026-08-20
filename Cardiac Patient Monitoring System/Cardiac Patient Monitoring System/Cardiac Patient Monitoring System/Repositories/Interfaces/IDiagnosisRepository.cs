// This interface defines the database operations for the Diagnosis repository.
// It handles retrieving, creating, updating, deleting, and filtering diagnoses.

using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IDiagnosisRepository
    {
        // Returns a diagnosis by its ID.
        Task<Diagnosis?> GetByIdAsync(int id);

        // Returns diagnoses belonging to a specific patient.
        Task<IEnumerable<Diagnosis>> GetByPatientIdAsync(int patientId);

        // Returns all diagnoses.
        Task<IEnumerable<Diagnosis>> GetAllAsync();

        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(int userId);

        // Creates a new diagnosis.
        Task<Diagnosis> AddAsync(Diagnosis diagnosis);

        // Updates an existing diagnosis.
        Task UpdateAsync(Diagnosis diagnosis);

        // Deletes an existing diagnosis.
        Task DeleteAsync(Diagnosis diagnosis);

        // Filters diagnoses based on patient and diagnosis information.
        Task<IEnumerable<Diagnosis>> FilterAsync(
            string? patientName,
            int? age,
            string? gender,
            string? nationalId,
            string? diagnosisName);

        // Returns diagnoses belonging to the authenticated user.
        Task<IEnumerable<Diagnosis>> GetByUserIdAsync(int userId);
    }
}