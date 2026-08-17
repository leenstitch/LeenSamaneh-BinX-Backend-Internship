using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;

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
    }
}
