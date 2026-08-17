// This interface defines the data access operations required for patients.
// It provides a contract for retrieving, updating, deleting, and saving patient data.
// The service layer depends on this interface instead of directly accessing the database.

using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public interface IPatientRepository
    {
        // Retrieves a patient using the User ID associated with the patient.
        Task<Patient?> GetByUserIdAsync(int userId);

        // Retrieves a patient using the Patient ID.
        Task<Patient?> GetByIdAsync(int patientId);

        // Retrieves all patients from the database.
        Task<List<Patient>> GetAllAsync();

        // Marks the patient entity as modified so the changes can be saved.
        Task UpdateAsync(Patient patient);

        // Marks the patient entity for deletion.
        Task DeleteAsync(Patient patient);

        // Saves all pending changes to the database.
        Task SaveChangesAsync();
    }
}