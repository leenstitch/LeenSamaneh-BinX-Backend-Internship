using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.PatientDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PatientResponseDto?> GetMyProfileAsync(
            int userId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

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
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (patient == null)
                return null;

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.PatientGender = dto.PatientGender;
            patient.PrimaryPhone = dto.PrimaryPhone;
            patient.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

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
            return await _context.Patients
                .Select(patient => new PatientResponseDto
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
                })
                .ToListAsync();
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null)
                return false;

            _context.Patients.Remove(patient);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}