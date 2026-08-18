using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Repositories;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientResponseDto?> GetMyProfileAsync(int userId)
        {
            var patient =
                await _patientRepository.GetByUserIdAsync(userId);

            if (patient == null)
                return null;

            return new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PatientGender = patient.PatientGender.ToString(),
                PrimaryPhone = patient.PrimaryPhone,
                NationalId = patient.NationalId,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }

        public async Task<PatientResponseDto?> UpdateMyProfileAsync(
            int userId,
            UpdatePatientDto dto)
        {
            var patient =
                await _patientRepository.GetByUserIdAsync(userId);

            if (patient == null)
                return null;

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.PatientGender = dto.PatientGender;
            patient.PrimaryPhone = dto.PrimaryPhone;
            patient.UpdatedAt = DateTime.UtcNow;

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync();

            return new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PatientGender = patient.PatientGender.ToString(),
                PrimaryPhone = patient.PrimaryPhone,
                NationalId = patient.NationalId,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }

        public async Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync()
        {
            var patients =
                await _patientRepository.GetAllAsync();

            return patients.Select(patient => new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PatientGender = patient.PatientGender.ToString(),
                PrimaryPhone = patient.PrimaryPhone,
                NationalId = patient.NationalId,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            });
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            var patient =
                await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
                return false;

            await _patientRepository.DeleteAsync(patient);
            await _patientRepository.SaveChangesAsync();

            return true;
        }
        public async Task<PatientResponseDto?> GetPatientByIdAsync(
    int patientId)
        {
            var patient =
                await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
                return null;

            return new PatientResponseDto
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PatientGender = patient.PatientGender.ToString(),
                PrimaryPhone = patient.PrimaryPhone,
                NationalId = patient.NationalId,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }
    }
}