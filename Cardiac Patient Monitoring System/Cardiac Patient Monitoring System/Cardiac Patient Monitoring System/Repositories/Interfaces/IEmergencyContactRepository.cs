// This interface defines the database operations for the Emergency Contact repository.
// It handles retrieving, creating, updating, deleting, and linking contacts to patients.

using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IEmergencyContactRepository
    {
        // Returns an emergency contact by its ID.
        Task<EmergencyContact?> GetByIdAsync(int id);

        // Returns emergency contacts belonging to a specific patient.
        Task<IEnumerable<EmergencyContact>> GetByPatientIdAsync(
            int patientId);

        // Returns all emergency contacts.
        Task<IEnumerable<EmergencyContact>> GetAllAsync();

        // Creates a new emergency contact.
        Task<EmergencyContact> AddAsync(
            EmergencyContact emergencyContact);

        // Updates an existing emergency contact.
        Task UpdateAsync(
            EmergencyContact emergencyContact);

        // Deletes an existing emergency contact.
        Task DeleteAsync(
            EmergencyContact emergencyContact);

        // Returns emergency contacts belonging to the authenticated user.
        Task<IEnumerable<EmergencyContact>> GetByUserIdAsync(
            int userId);

        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(int userId);
    }
}