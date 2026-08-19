using Cardiac_Patient_Monitoring_System.DTO_S.VitalSignDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IVitalSignService
    {
        Task<VitalSignResponseDto?> GetByIdAsync(int id);

        Task<IEnumerable<VitalSignResponseDto>>
            GetByPatientIdAsync(int patientId);

        Task<IEnumerable<VitalSignResponseDto>>
            GetAllAsync();

        Task<VitalSignResponseDto>
            CreateAsync(CreateVitalSignDto dto);

        Task<IEnumerable<VitalSignResponseDto>>
            GetMyVitalSignsAsync(int userId);

        Task<IEnumerable<VitalSignResponseDto>>
            FilterAsync(VitalSignFilterDto filter);

        Task<bool> UpdateAsync(
            int id,
            UpdateVitalSignDto dto);

        Task<bool> DeleteAsync(int id);
        Task<VitalSignComparisonDto?> CompareLatestTwoAsync(int userId);
        Task<VitalSignDateComparisonDto?>
    CompareByDatesAsync(
        int userId,
        DateTime firstDate,
        DateTime secondDate);
    }
}