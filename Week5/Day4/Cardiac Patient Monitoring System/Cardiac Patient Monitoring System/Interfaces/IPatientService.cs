using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.Summary;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IPatientService
    {
        Task<PatientResponseDto?> GetMyProfileAsync(int userId);

        Task<PatientResponseDto?> UpdateMyProfileAsync(
            int userId,
            UpdatePatientDto dto);

        Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();

        Task<bool> DeletePatientAsync(int patientId);
        Task<PatientHealthSummaryDto?>
    GetMyHealthSummaryAsync(int userId);

        Task<PatientHealthSummaryDto?>
            GetHealthSummaryAsync(int patientId);
        Task<PatientHealthStatusDto?>
    GetMyHealthStatusAsync(int userId);

        Task<PatientHealthStatusDto?>
            GetHealthStatusAsync(int patientId);
    }
}
