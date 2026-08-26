using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IMedicalProcedureRepository
    {

        // Returns medical procedures for a patient
        // within the cardiac-event analysis date range.
        Task<IEnumerable<MedicalProcedure>>
            GetByPatientAndDateRangeAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate);

        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(
            int userId);

        // Creates a new medical procedure record.
        Task<MedicalProcedure> AddAsync(
            MedicalProcedure procedure);
    }
}