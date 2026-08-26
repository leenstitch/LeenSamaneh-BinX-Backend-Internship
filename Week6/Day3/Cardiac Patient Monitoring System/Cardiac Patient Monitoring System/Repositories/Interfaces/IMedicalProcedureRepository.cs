using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IMedicalProcedureRepository
    {
        Task<IEnumerable<MedicalProcedure>>  GetByPatientAndDateRangeAsync(
         int patientId,
         DateTime startDate,
         DateTime eventDate);
    }
}
