// This repository handles database operations for patient vital signs.
// It provides methods for retrieving, creating, updating, deleting,
// filtering, and comparing vital-sign records.

using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class VitalSignRepository
        : IVitalSignRepository
    {
        private readonly ApplicationDbContext _context;

        public VitalSignRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves a vital-sign record by its ID.
        public async Task<VitalSign?> GetByIdAsync(int id)
        {
            return await _context.VitalSigns
                .Include(v => v.Patient)
                .FirstOrDefaultAsync(
                    v => v.VitalSignId == id);
        }

        // Retrieves vital-sign records for a specific patient.
        public async Task<IEnumerable<VitalSign>>
            GetByPatientIdAsync(int patientId)
        {
            return await _context.VitalSigns
                .Where(v => v.PatientId == patientId)
                .OrderByDescending(v => v.MeasuredAt)
                .ToListAsync();
        }

        // Retrieves all vital-sign records.
        public async Task<IEnumerable<VitalSign>>
            GetAllAsync()
        {
            return await _context.VitalSigns
                .Include(v => v.Patient)
                .OrderByDescending(v => v.MeasuredAt)
                .ToListAsync();
        }

        // Adds a new vital-sign record and saves it to the database.
        public async Task<VitalSign>
            AddAsync(VitalSign vitalSign)
        {
            await _context.VitalSigns.AddAsync(vitalSign);

            await _context.SaveChangesAsync();

            return vitalSign;
        }

        // Updates an existing vital-sign record.
        public async Task UpdateAsync(VitalSign vitalSign)
        {
            _context.VitalSigns.Update(vitalSign);

            await _context.SaveChangesAsync();
        }

        // Deletes an existing vital-sign record.
        public async Task DeleteAsync(VitalSign vitalSign)
        {
            _context.VitalSigns.Remove(vitalSign);

            await _context.SaveChangesAsync();
        }

        // Filters vital signs using patient information such as
        // name, age, gender, and national ID.
        public async Task<IEnumerable<VitalSign>>
            FilterAsync(
                string? patientName,
                int? age,
                string? gender,
                string? nationalId)
        {
            var query = _context.VitalSigns
                .Include(v => v.Patient)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(patientName))
            {
                query = query.Where(v =>
                    v.Patient.FirstName.Contains(patientName) ||
                    v.Patient.LastName.Contains(patientName));
            }

            if (age.HasValue)
            {
                var today = DateTime.Today;

                query = query.Where(v =>
                    today.Year -
                    v.Patient.DateOfBirth.Year -
                    (
                        today <
                        v.Patient.DateOfBirth.AddYears(
                            today.Year -
                            v.Patient.DateOfBirth.Year)
                            ? 1
                            : 0
                    )
                    == age.Value);
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                if (Enum.TryParse<Patient.Gender>(
                    gender,
                    true,
                    out var parsedGender))
                {
                    query = query.Where(v =>
                        v.Patient.PatientGender ==
                        parsedGender);
                }
            }

            if (!string.IsNullOrWhiteSpace(nationalId))
            {
                query = query.Where(v =>
                    v.Patient.NationalId.Contains(nationalId));
            }

            return await query
                .OrderByDescending(v => v.MeasuredAt)
                .ToListAsync();
        }

        // Retrieves vital-sign records belonging to the authenticated user.
        public async Task<IEnumerable<VitalSign>>
            GetByUserIdAsync(int userId)
        {
            return await _context.VitalSigns
                .Where(v => v.Patient.UserId == userId)
                .OrderByDescending(v => v.MeasuredAt)
                .ToListAsync();
        }

        // Retrieves the two most recent vital-sign records for a user.
        public async Task<List<VitalSign>> GetLatestTwoByUserIdAsync(
            int userId)
        {
            return await _context.VitalSigns
                .Where(v => v.Patient.UserId == userId)
                .OrderByDescending(v => v.MeasuredAt)
                .Take(2)
                .ToListAsync();
        }

        // Retrieves the latest vital-sign record recorded on a specific date.
        public async Task<VitalSign?>
            GetLatestByUserIdAndDateAsync(
                int userId,
                DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _context.VitalSigns
                .Where(v =>
                    v.Patient.UserId == userId &&
                    v.MeasuredAt >= startDate &&
                    v.MeasuredAt < endDate)
                .OrderByDescending(v => v.VitalSignId)
                .FirstOrDefaultAsync();
        }

        // Finds the PatientId associated with the specified UserId.
        public async Task<int?> GetPatientIdByUserIdAsync(
            int userId)
        {
            return await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.PatientId)
                .FirstOrDefaultAsync();
        }
    }
}