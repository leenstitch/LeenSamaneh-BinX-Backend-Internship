using Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s;
using Cardiac_Patient_Monitoring_System.DTOs;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IDiagnosisService
    {
        Task<DiagnosisResponseDto?> GetByIdAsync(int id);

        Task<IEnumerable<DiagnosisResponseDto?>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<DiagnosisResponseDto?>> GetAllAsync();

        Task<Diagnosis?> CreateAsync(int userId, CreateDiagnosisDto dto);

        Task<IEnumerable<DiagnosisResponseDto?>> GetMyDiagnosesAsync(int userId);

        Task<bool> UpdateAsync(
            int id,
            UpdateDiagnosisDto dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<DiagnosisResponseDto>> FilterAsync(
       DiagnosisFilterDto filter);
    }
}