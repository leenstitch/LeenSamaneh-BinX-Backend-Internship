using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IDiagnosisRepository
    {
        Task<Diagnosis?> GetByIdAsync(int id);

        Task<IEnumerable<Diagnosis>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<Diagnosis>> GetAllAsync();

        Task<int?> GetPatientIdByUserIdAsync(int userId);

        Task<Diagnosis> AddAsync(Diagnosis diagnosis);

        Task UpdateAsync(Diagnosis diagnosis);

        Task DeleteAsync(Diagnosis diagnosis);

        Task<IEnumerable<Diagnosis>> FilterAsync(
            string? patientName,
            int? age,
            string? gender,
            string? nationalId,
            string? diagnosisName
            );

        Task<IEnumerable<Diagnosis>> GetByUserIdAsync(int userId);
    }
}