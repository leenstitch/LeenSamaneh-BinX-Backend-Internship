using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface ILabResultRepository
    {
        public interface ILabResultRepository
        {
            Task<IEnumerable<LabResult>>
           GetByPatientAndDateRangeAsync(
               int patientId,
               DateTime startDate,
               DateTime endDate);
        }
    }
}