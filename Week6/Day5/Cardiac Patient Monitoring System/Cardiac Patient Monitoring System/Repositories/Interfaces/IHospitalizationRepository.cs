using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IHospitalizationRepository
    {
        // Returns hospitalizations that overlap with the specified date range.
        Task<IEnumerable<Hospitalization>> GetOverlappingPeriodAsync(
        int patientId,
        DateTime startDate,
        DateTime eventDate);


        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(int userId);


        // Creates a new hospitalization record.
        Task<Hospitalization> AddAsync(
            Hospitalization hospitalization);
    }
}
