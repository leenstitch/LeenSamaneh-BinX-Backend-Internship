// This interface defines the operations provided by the Diagnosis service.
// It handles retrieving, creating, filtering, updating, and deleting diagnoses.

using Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s;
using Cardiac_Patient_Monitoring_System.DTOs;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IDiagnosisService
    {
        // Returns a diagnosis by its ID.
        Task<DiagnosisResponseDto?> GetByIdAsync(int id);

        // Returns diagnoses belonging to a specific patient.
        Task<IEnumerable<DiagnosisResponseDto?>> GetByPatientIdAsync(int patientId);

        // Returns all diagnoses.
        Task<IEnumerable<DiagnosisResponseDto?>> GetAllAsync();

        // Creates a diagnosis for the patient linked to the authenticated user.
        Task<Diagnosis?> CreateAsync(
            int userId,
            CreateDiagnosisDto dto);

        // Returns diagnoses belonging to the authenticated patient.
        Task<IEnumerable<DiagnosisResponseDto?>> GetMyDiagnosesAsync(int userId);

        // Updates an existing diagnosis.
        Task<bool> UpdateAsync(
            int id,
            UpdateDiagnosisDto dto);

        // Deletes an existing diagnosis.
        Task<bool> DeleteAsync(int id);

        // Filters diagnoses based on the provided filter criteria.
        Task<IEnumerable<DiagnosisResponseDto>> FilterAsync(
            DiagnosisFilterDto filter);
    }
}