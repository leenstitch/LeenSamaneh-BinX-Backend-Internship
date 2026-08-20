using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IMedicationService
    {
        Task<MedicationResponseDto?> GetByIdAsync(int id);

        Task<IEnumerable<MedicationResponseDto>>
            GetByPatientIdAsync(int patientId);

        Task<IEnumerable<MedicationResponseDto>>
            GetMyMedicationsAsync(int userId);

        Task<IEnumerable<MedicationResponseDto>>
            GetAllAsync();

        Task<MedicationResponseDto?> CreateAsync(
    int userId,
    CreateMedicationDto dto);

        Task<bool>
            UpdateAsync(int id, UpdateMedicationDto dto);

        Task<bool>
            DeleteAsync(int id);
        Task<IEnumerable<MedicationResponseDto>>
    FilterMyMedicationsAsync(
        int userId,
        MedicationFilterDto filter);
    }
}