// This repository handles database operations for diagnoses.
// It provides methods for retrieving, creating, updating, deleting,
// filtering, and linking diagnoses to patients.

using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class DiagnosisRepository : IDiagnosisRepository
    {
        private readonly ApplicationDbContext _context;

        public DiagnosisRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves a diagnosis by its ID.
        public async Task<Diagnosis?> GetByIdAsync(int id)
        {
            return await _context.Diagnoses
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(d => d.DiagnosisId == id);
        }

        // Retrieves all diagnoses for a specific patient.
        public async Task<IEnumerable<Diagnosis>> GetByPatientIdAsync(
            int patientId)
        {
            return await _context.Diagnoses
                .Where(d => d.PatientId == patientId)
                .OrderByDescending(d => d.DiagnosedAt)
                .ToListAsync();
        }

        // Retrieves all diagnoses.
        public async Task<IEnumerable<Diagnosis>> GetAllAsync()
        {
            return await _context.Diagnoses
                .Include(d => d.Patient)
                .OrderByDescending(d => d.DiagnosedAt)
                .ToListAsync();
        }

        // Adds a new diagnosis and saves it to the database.
        public async Task<Diagnosis> AddAsync(Diagnosis diagnosis)
        {
            await _context.Diagnoses.AddAsync(diagnosis);
            await _context.SaveChangesAsync();

            return diagnosis;
        }

        // Finds the PatientId associated with the specified UserId.
        public async Task<int?> GetPatientIdByUserIdAsync(int userId)
        {
            return await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.PatientId)
                .FirstOrDefaultAsync();
        }

        // Updates an existing diagnosis and saves the changes.
        public async Task UpdateAsync(Diagnosis diagnosis)
        {
            _context.Diagnoses.Update(diagnosis);
            await _context.SaveChangesAsync();
        }

        // Deletes an existing diagnosis and saves the changes.
        public async Task DeleteAsync(Diagnosis diagnosis)
        {
            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();
        }

        // Filters diagnoses using patient and diagnosis information.
        public async Task<IEnumerable<Diagnosis>> FilterAsync(
            string? patientName,
            int? age,
            string? gender,
            string? nationalId,
            string? diagnosisName)
        {
            var query = _context.Diagnoses
                .Include(d => d.Patient)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(patientName))
            {
                query = query.Where(d =>
                    d.Patient.FirstName.Contains(patientName) ||
                    d.Patient.LastName.Contains(patientName));
            }

            if (age.HasValue)
            {
                var today = DateTime.Today;

                query = query.Where(d =>
                    today.Year - d.Patient.DateOfBirth.Year -
                    (today < d.Patient.DateOfBirth.AddYears(
                        today.Year - d.Patient.DateOfBirth.Year)
                        ? 1
                        : 0)
                    == age.Value);
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(d =>
                    d.Patient.PatientGender.Equals(gender));
            }

            if (!string.IsNullOrWhiteSpace(nationalId))
            {
                query = query.Where(d =>
                    d.Patient.NationalId.Equals(nationalId));
            }

            if (!string.IsNullOrWhiteSpace(diagnosisName))
            {
                query = query.Where(d =>
                    d.DiagnosisName.Contains(diagnosisName));
            }

            return await query
                .OrderByDescending(d => d.DiagnosedAt)
                .ToListAsync();
        }

        // Retrieves diagnoses belonging to the authenticated user.
        public async Task<IEnumerable<Diagnosis>> GetByUserIdAsync(
            int userId)
        {
            return await _context.Diagnoses
                .Include(d => d.Patient)
                .Where(d => d.Patient.UserId == userId)
                .OrderByDescending(d => d.DiagnosedAt)
                .ToListAsync();
        }


        // Retrieves all diagnoses recorded for a patient
        // before or on the date of a specific cardiac event.
        // No tracking is required because the records are read-only.
        public async Task<IEnumerable<Diagnosis>>
    GetRecordedBeforeEventAsync(
        int patientId,
        DateTime eventDate)
        {
            return await _context.Diagnoses
                .AsNoTracking()
                .Where(x =>
                    x.PatientId == patientId &&
                    x.DiagnosedAt <= eventDate)
                .OrderByDescending(x => x.DiagnosedAt)
                .ToListAsync();
        }
    }
}