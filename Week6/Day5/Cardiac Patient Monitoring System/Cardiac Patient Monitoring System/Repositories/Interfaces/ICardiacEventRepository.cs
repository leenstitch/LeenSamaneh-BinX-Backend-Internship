using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface ICardiacEventRepository
    {

        // Returns a cardiac event by its ID.
        Task<CardiacEvent?> GetByIdAsync(int id);


        // Returns previous cardiac events for a patient
        // within the specified date range.
        Task<IEnumerable<CardiacEvent>> GetPreviousEventsAsync(
            int patientId,
            DateTime startDate,
            DateTime eventDate);
        // Finds the PatientId associated with the specified UserId.
        Task<int?> GetPatientIdByUserIdAsync(
             int userId);


        // Creates a new cardiac event record.
        Task<CardiacEvent> AddAsync(
            CardiacEvent cardiacEvent);
    }
}