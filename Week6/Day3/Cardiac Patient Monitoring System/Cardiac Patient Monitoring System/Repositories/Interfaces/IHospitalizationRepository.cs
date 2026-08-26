using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IHospitalizationRepository
    {
        Task<IEnumerable<Hospitalization>> GetOverlappingPeriodAsync(
        int patientId,
        DateTime startDate,
        DateTime eventDate);
    }
}
