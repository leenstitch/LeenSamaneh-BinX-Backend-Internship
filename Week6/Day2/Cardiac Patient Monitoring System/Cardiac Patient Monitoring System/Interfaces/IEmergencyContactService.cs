// This interface defines the operations provided by the Emergency Contact service.
// It handles retrieving, creating, updating, and deleting emergency contacts.

using Cardiac_Patient_Monitoring_System.DTO_S.EmergencyContactDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IEmergencyContactService
    {
        // Returns an emergency contact by its ID.
        Task<EmergencyContactResponseDto?> GetByIdAsync(int id);

        // Returns emergency contacts belonging to a specific patient.
        Task<IEnumerable<EmergencyContactResponseDto>>
            GetByPatientIdAsync(int patientId);

        // Returns all emergency contacts.
        Task<IEnumerable<EmergencyContactResponseDto>>
            GetAllAsync();

        // Creates an emergency contact for the patient linked to the user.
        Task<EmergencyContactResponseDto?> CreateAsync(
            int userId,
            CreateEmergencyContactDto dto);

        // Returns emergency contacts belonging to the authenticated patient.
        Task<IEnumerable<EmergencyContactResponseDto>>
            GetMyEmergencyContactsAsync(int userId);

        // Updates an existing emergency contact.
        Task<bool> UpdateAsync(
            int id,
            UpdateEmergencyContactDto dto);

        // Deletes an existing emergency contact.
        Task<bool> DeleteAsync(int id);
    }
}