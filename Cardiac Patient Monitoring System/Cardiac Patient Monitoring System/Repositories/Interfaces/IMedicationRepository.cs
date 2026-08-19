using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public interface IMedicationRepository
    {
        Task<Medication?> GetByIdAsync(int id);

        Task<IEnumerable<Medication>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<Medication>> GetByUserIdAsync(int userId);

        Task<IEnumerable<Medication>> GetAllAsync();

        Task<Medication> AddAsync(Medication medication);

        Task UpdateAsync(Medication medication);

        Task DeleteAsync(Medication medication);
        Task<int?> GetPatientIdByUserIdAsync(int userId);
        Task<IEnumerable<Medication>>
           FilterByUserIdAsync(
               int userId,
               MedicationFilterDto filter);
    }
}