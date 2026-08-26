using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface ILabResultRepository
    {
        // Returns all lab results for a patient
        // within the cardiac-event analysis date range.
        Task<IEnumerable<LabResult>>
            GetForCardiacEventAnalysisAsync(
                int patientId,
                DateTime startDate,
                DateTime endDate);
        // Creates a new lab result record.
        Task<LabResult> AddAsync(
            LabResult labResult);

        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(
            int userId);
    }
}