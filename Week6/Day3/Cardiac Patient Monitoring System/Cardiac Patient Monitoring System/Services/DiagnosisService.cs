// This service handles the business logic for patient diagnoses.
// It provides operations for retrieving, creating, filtering,
// updating, deleting, and retrieving diagnoses for the authenticated patient.

using Cardiac_Patient_Monitoring_System.DTO_S.DiagnosisDto_s;
using Cardiac_Patient_Monitoring_System.DTOs;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly IDiagnosisRepository _repository;

        public DiagnosisService(IDiagnosisRepository repository)
        {
            _repository = repository;
        }

        // Returns a diagnosis by its ID.
        public async Task<DiagnosisResponseDto?> GetByIdAsync(int id)
        {
            var diagnosis =
                await _repository.GetByIdAsync(id);

            if (diagnosis == null)
                return null;

            return new DiagnosisResponseDto
            {
                DiagnosisId = diagnosis.DiagnosisId,
                PatientId = diagnosis.PatientId,
                DiagnosisName = diagnosis.DiagnosisName,
                DiagnosedByName = diagnosis.DiagnosedByName,
                DiagnosedBySpecialization = diagnosis.DiagnosedBySpecialization,
                DiagnosedAt = diagnosis.DiagnosedAt,
                Notes = diagnosis.Notes,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
            };
        }

        // Returns diagnoses belonging to a specific patient.
        public async Task<IEnumerable<DiagnosisResponseDto>> GetByPatientIdAsync(
            int patientId)
        {
            var diagnoses =
                await _repository.GetByPatientIdAsync(patientId);

            return diagnoses.Select(diagnosis => new DiagnosisResponseDto
            {
                DiagnosisId = diagnosis.DiagnosisId,
                PatientId = diagnosis.PatientId,
                DiagnosisName = diagnosis.DiagnosisName,
                DiagnosedByName = diagnosis.DiagnosedByName,
                DiagnosedBySpecialization = diagnosis.DiagnosedBySpecialization,
                DiagnosedAt = diagnosis.DiagnosedAt,
                Notes = diagnosis.Notes,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
            });
        }

        // Returns all diagnoses.
        public async Task<IEnumerable<DiagnosisResponseDto>> GetAllAsync()
        {
            var diagnoses =
                await _repository.GetAllAsync();

            return diagnoses.Select(diagnosis => new DiagnosisResponseDto
            {
                DiagnosisId = diagnosis.DiagnosisId,
                PatientId = diagnosis.PatientId,
                DiagnosisName = diagnosis.DiagnosisName,
                DiagnosedByName = diagnosis.DiagnosedByName,
                DiagnosedAt = diagnosis.DiagnosedAt,
                DiagnosedBySpecialization = diagnosis.DiagnosedBySpecialization,
                Notes = diagnosis.Notes,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
            });
        }

        // Creates a diagnosis for the patient linked to the authenticated user.
        public async Task<Diagnosis?> CreateAsync(
            int userId,
            CreateDiagnosisDto dto)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return null;

            var diagnosis = new Diagnosis
            {
                PatientId = patientId.Value,
                DiagnosisName = dto.DiagnosisName,
                DiagnosedAt = dto.DiagnosedAt,
                DiagnosedByName = dto.DiagnosedByName,
                DiagnosedBySpecialization = dto.DiagnosedBySpecialization,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(diagnosis);
        }

        // Updates an existing diagnosis.
        public async Task<bool> UpdateAsync(
            int id,
            UpdateDiagnosisDto dto)
        {
            var diagnosis =
                await _repository.GetByIdAsync(id);

            if (diagnosis == null)
                return false;

            if (dto.DiagnosedBySpecialization != null)
                diagnosis.DiagnosedBySpecialization =
                    dto.DiagnosedBySpecialization;

            if (dto.DiagnosedByName != null)
                diagnosis.DiagnosedByName =
                    dto.DiagnosedByName;

            if (dto.DiagnosisName != null)
                diagnosis.DiagnosisName =
                    dto.DiagnosisName;

            if (dto.DiagnosedAt.HasValue)
                diagnosis.DiagnosedAt =
                    dto.DiagnosedAt.Value;

            if (dto.Notes != null)
                diagnosis.Notes =
                    dto.Notes;

            diagnosis.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(diagnosis);

            return true;
        }

        // Deletes an existing diagnosis.
        public async Task<bool> DeleteAsync(int id)
        {
            var diagnosis =
                await _repository.GetByIdAsync(id);

            if (diagnosis == null)
                return false;

            await _repository.DeleteAsync(diagnosis);

            return true;
        }

        // Filters diagnoses using patient and diagnosis information.
        public async Task<IEnumerable<DiagnosisResponseDto>> FilterAsync(
            DiagnosisFilterDto filter)
        {
            var diagnoses =
                await _repository.FilterAsync(
                    filter.PatientName,
                    filter.Age,
                    filter.Gender,
                    filter.NationalId,
                    filter.DiagnosisName);

            return diagnoses.Select(diagnosis => new DiagnosisResponseDto
            {
                DiagnosisId = diagnosis.DiagnosisId,
                PatientId = diagnosis.PatientId,
                DiagnosisName = diagnosis.DiagnosisName,
                DiagnosedByName = diagnosis.DiagnosisName,
                DiagnosedAt = diagnosis.DiagnosedAt,
                DiagnosedBySpecialization = diagnosis.DiagnosisName,
                Notes = diagnosis.Notes,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
            });
        }

        // Returns diagnoses belonging to the authenticated patient.
        public async Task<IEnumerable<DiagnosisResponseDto>> GetMyDiagnosesAsync(
            int userId)
        {
            var diagnoses =
                await _repository.GetByUserIdAsync(userId);

            return diagnoses.Select(diagnosis => new DiagnosisResponseDto
            {
                DiagnosisId = diagnosis.DiagnosisId,
                PatientId = diagnosis.PatientId,
                DiagnosisName = diagnosis.DiagnosisName,
                DiagnosedByName = diagnosis.DiagnosisName,
                DiagnosedAt = diagnosis.DiagnosedAt,
                DiagnosedBySpecialization =
                    diagnosis.DiagnosedBySpecialization,
                Notes = diagnosis.Notes,
                CreatedAt = diagnosis.CreatedAt,
                UpdatedAt = diagnosis.UpdatedAt
            });
        }
    }
}