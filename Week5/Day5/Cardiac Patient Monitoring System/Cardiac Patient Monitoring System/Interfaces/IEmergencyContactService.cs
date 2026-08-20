using Cardiac_Patient_Monitoring_System.DTO_S.EmergencyContactDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IEmergencyContactService
    {
        Task<EmergencyContactResponseDto?> GetByIdAsync(int id);

        Task<IEnumerable<EmergencyContactResponseDto>>
            GetByPatientIdAsync(int patientId);

        Task<IEnumerable<EmergencyContactResponseDto>>
            GetAllAsync();

        Task<EmergencyContactResponseDto?> CreateAsync(
            int userId,
            CreateEmergencyContactDto dto);

        Task<IEnumerable<EmergencyContactResponseDto>>
            GetMyEmergencyContactsAsync(int userId);

        Task<bool> UpdateAsync(
            int id,
            UpdateEmergencyContactDto dto);

        Task<bool> DeleteAsync(int id);
    }
}