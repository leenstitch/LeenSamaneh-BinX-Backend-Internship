using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IEmergencyContactRepository
    {
        Task<EmergencyContact?> GetByIdAsync(int id);

        Task<IEnumerable<EmergencyContact>> GetByPatientIdAsync(
            int patientId);

        Task<IEnumerable<EmergencyContact>> GetAllAsync();

        Task<EmergencyContact> AddAsync(
            EmergencyContact emergencyContact);

        Task UpdateAsync(
            EmergencyContact emergencyContact);

        Task DeleteAsync(
            EmergencyContact emergencyContact);

        Task<IEnumerable<EmergencyContact>> GetByUserIdAsync(
            int userId);

        Task<int?> GetPatientIdByUserIdAsync(int userId);
    }
}