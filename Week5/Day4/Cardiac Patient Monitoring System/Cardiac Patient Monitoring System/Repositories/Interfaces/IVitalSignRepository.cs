using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IVitalSignRepository
    {
        Task<VitalSign?> GetByIdAsync(int id);

        Task<IEnumerable<VitalSign>>
            GetByPatientIdAsync(int patientId);

        Task<IEnumerable<VitalSign>>
            GetAllAsync();

        Task<VitalSign>
            AddAsync(VitalSign vitalSign);

        Task UpdateAsync(VitalSign vitalSign);

        Task DeleteAsync(VitalSign vitalSign);

        Task<IEnumerable<VitalSign>> FilterAsync(
            string? patientName,
            int? age,
            string? gender,
            string? nationalId);

        Task<IEnumerable<VitalSign>>
            GetByUserIdAsync(int userId);
        Task<List<VitalSign>> GetLatestTwoByUserIdAsync(int userId);
        Task<VitalSign?> GetLatestByUserIdAndDateAsync(
    int userId,
    DateTime date);
    }
}