// This repository handles database operations for medications.
// It provides methods for retrieving, creating, updating, deleting,
// and filtering medications.

using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.MedicationDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves a medication by its ID.
        public async Task<Medication?> GetByIdAsync(int id)
        {
            return await _context.Medications
                .FirstOrDefaultAsync(m => m.MedicationId == id);
        }

        // Retrieves all medications for a specific patient.
        public async Task<IEnumerable<Medication>> GetByPatientIdAsync(
            int patientId)
        {
            return await _context.Medications
                .Where(m => m.PatientId == patientId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // Retrieves medications belonging to the authenticated user.
        public async Task<IEnumerable<Medication>> GetByUserIdAsync(
            int userId)
        {
            return await _context.Medications
                .Where(m => m.Patient.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // Retrieves all medications.
        public async Task<IEnumerable<Medication>> GetAllAsync()
        {
            return await _context.Medications
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // Adds a new medication and saves it to the database.
        public async Task<Medication> AddAsync(
            Medication medication)
        {
            await _context.Medications.AddAsync(medication);
            await _context.SaveChangesAsync();

            return medication;
        }

        // Updates an existing medication and saves the changes.
        public async Task UpdateAsync(Medication medication)
        {
            _context.Medications.Update(medication);
            await _context.SaveChangesAsync();
        }

        // Deletes an existing medication and saves the changes.
        public async Task DeleteAsync(Medication medication)
        {
            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();
        }

        // Finds the PatientId associated with the specified UserId.
        public async Task<int?> GetPatientIdByUserIdAsync(int userId)
        {
            return await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.PatientId)
                .FirstOrDefaultAsync();
        }

        // Filters medications for the authenticated user based on
        // name, start date, end date, and medication status.
        public async Task<IEnumerable<Medication>>
            FilterByUserIdAsync(
                int userId,
                MedicationFilterDto filter)
        {
            var query = _context.Medications
                .Where(m => m.Patient.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(m =>
                    m.Name.Contains(filter.Name));
            }

            if (filter.StartDate.HasValue)
            {
                var startDate = filter.StartDate.Value.Date;
                var endDate = startDate.AddDays(1);

                query = query.Where(m =>
                    m.StartDate >= startDate &&
                    m.StartDate < endDate);
            }

            if (filter.EndDate.HasValue)
            {
                var startDate = filter.EndDate.Value.Date;
                var endDate = startDate.AddDays(1);

                query = query.Where(m =>
                    m.EndDate.HasValue &&
                    m.EndDate.Value >= startDate &&
                    m.EndDate.Value < endDate);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                var today = DateTime.UtcNow.Date;

                if (filter.Status.Equals(
                    "Active",
                    StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(m =>
                        m.StartDate.Date <= today &&
                        (!m.EndDate.HasValue ||
                         m.EndDate.Value.Date >= today));
                }
                else if (filter.Status.Equals(
                    "Expired",
                    StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(m =>
                        m.EndDate.HasValue &&
                        m.EndDate.Value.Date < today);
                }
                else if (filter.Status.Equals(
                    "Upcoming",
                    StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(m =>
                        m.StartDate.Date > today);
                }
            }

            return await query
                .OrderByDescending(m => m.StartDate)
                .ToListAsync();
        }

        // Retrieves medications that were active or overlapped
        // with the specified cardiac-event analysis period.
        public async Task<IEnumerable<Medication>>
    GetHistoricalMedicationsAsync(
        int patientId,
        DateTime startDate,
        DateTime eventDate)
        {
            return await _context.Medications
                .AsNoTracking()
                .Where(x =>
                    x.PatientId == patientId &&
                    x.StartDate <= eventDate &&
                    (
                        x.EndDate == null ||
                        x.EndDate >= startDate
                    ))
                .OrderBy(x => x.StartDate)
                .ToListAsync();
        }
    }
}