using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repository;

        public MedicationService(
            IMedicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<MedicationResponseDto?> GetByIdAsync(int id)
        {
            var medication =
                await _repository.GetByIdAsync(id);

            if (medication == null)
                return null;

            return MapToDto(medication);
        }

        public async Task<IEnumerable<MedicationResponseDto>>
            GetByPatientIdAsync(int patientId)
        {
            var medications =
                await _repository.GetByPatientIdAsync(patientId);

            return medications.Select(MapToDto);
        }

        public async Task<IEnumerable<MedicationResponseDto>>
            GetMyMedicationsAsync(int userId)
        {
            var medications =
                await _repository.GetByUserIdAsync(userId);

            return medications.Select(MapToDto);
        }

        public async Task<IEnumerable<MedicationResponseDto>>
            GetAllAsync()
        {
            var medications =
                await _repository.GetAllAsync();

            return medications.Select(MapToDto);
        }

        public async Task<MedicationResponseDto?> CreateAsync(
     int userId,
     CreateMedicationDto dto)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return null;

            var medication = new Medication
            {
                PatientId = patientId.Value,
                PrescribedByDoctorName =
                    dto.PrescribedByDoctorName,
                PrescribedBySpecialization =
                    dto.PrescribedBySpecialization,
                Name = dto.Name,
                Dosage = dto.Dosage,
                Frequency = dto.Frequency,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdMedication =
                await _repository.AddAsync(medication);

            return new MedicationResponseDto
            {
                MedicationId =
                    createdMedication.MedicationId,

                PatientId =
                    createdMedication.PatientId,

                PrescribedByDoctorName =
                    createdMedication.PrescribedByDoctorName,

                PrescribedBySpecialization =
                    createdMedication.PrescribedBySpecialization,

                Name =
                    createdMedication.Name,

                Dosage =
                    createdMedication.Dosage,

                Frequency =
                    createdMedication.Frequency,

                StartDate =
                    createdMedication.StartDate,

                EndDate =
                    createdMedication.EndDate,

                Notes =
                    createdMedication.Notes,

                CreatedAt =
                    createdMedication.CreatedAt,

                UpdatedAt =
                    createdMedication.UpdatedAt
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateMedicationDto dto)
        {
            var medication =
                await _repository.GetByIdAsync(id);

            if (medication == null)
                return false;

            if (dto.PrescribedByDoctorName != null)
            {
                medication.PrescribedByDoctorName =
                    dto.PrescribedByDoctorName;
            }

            if (dto.PrescribedBySpecialization != null)
            {
                medication.PrescribedBySpecialization =
                    dto.PrescribedBySpecialization;
            }

            if (dto.Name != null)
            {
                medication.Name = dto.Name;
            }

            if (dto.Dosage != null)
            {
                medication.Dosage = dto.Dosage;
            }

            if (dto.Frequency != null)
            {
                medication.Frequency = dto.Frequency;
            }

            if (dto.StartDate.HasValue)
            {
                medication.StartDate =
                    dto.StartDate.Value;
            }

            if (dto.EndDate.HasValue)
            {
                medication.EndDate =
                    dto.EndDate.Value;
            }

            if (dto.Notes != null)
            {
                medication.Notes = dto.Notes;
            }

            medication.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(medication);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var medication =
                await _repository.GetByIdAsync(id);

            if (medication == null)
                return false;

            await _repository.DeleteAsync(medication);

            return true;
        }

        private static MedicationResponseDto MapToDto(
            Medication medication)
        {
            return new MedicationResponseDto
            {
                MedicationId = medication.MedicationId,
                PatientId = medication.PatientId,
                PrescribedByDoctorName =
                    medication.PrescribedByDoctorName,
                PrescribedBySpecialization =
                    medication.PrescribedBySpecialization,
                Name = medication.Name,
                Dosage = medication.Dosage,
                Frequency = medication.Frequency,
                StartDate = medication.StartDate,
                EndDate = medication.EndDate,
                Notes = medication.Notes,
                CreatedAt = medication.CreatedAt,
                UpdatedAt = medication.UpdatedAt
            };
        }
        public async Task<IEnumerable<MedicationResponseDto>>
    FilterMyMedicationsAsync(
        int userId,
        MedicationFilterDto filter)
        {
            var medications =
                await _repository.FilterByUserIdAsync(
                    userId,
                    filter);

            return medications.Select(medication =>
                new MedicationResponseDto
                {
                    MedicationId =
                        medication.MedicationId,

                    PatientId =
                        medication.PatientId,

                    PrescribedByDoctorName =
                        medication.PrescribedByDoctorName,

                    PrescribedBySpecialization =
                        medication.PrescribedBySpecialization,

                    Name =
                        medication.Name,

                    Dosage =
                        medication.Dosage,

                    Frequency =
                        medication.Frequency,

                    StartDate =
                        medication.StartDate,

                    EndDate =
                        medication.EndDate,

                    Notes =
                        medication.Notes,

                    CreatedAt =
                        medication.CreatedAt,

                    UpdatedAt =
                        medication.UpdatedAt
                });
        }
    }
}