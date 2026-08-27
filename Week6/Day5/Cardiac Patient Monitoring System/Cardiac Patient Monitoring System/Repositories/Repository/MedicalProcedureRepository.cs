using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class MedicalProcedureRepository
        : IMedicalProcedureRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicalProcedureRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }
        // Retrieves medical procedures for a specific patient
        // within the specified date range.

        public async Task<IEnumerable<MedicalProcedure>>
            GetByPatientAndDateRangeAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate)
        {
            return await _context.MedicalProcedures
                .AsNoTracking()
                .Where(x =>
                    x.PatientId == patientId &&
                    x.ProcedureDate >= startDate &&
                    x.ProcedureDate <= eventDate)
                .OrderBy(x => x.ProcedureDate)
                .ToListAsync();
        }


        // Adds a new medical procedure to the database
        // and saves the changes.
        public async Task<MedicalProcedure> AddAsync(
       MedicalProcedure procedure)
        {
            await _context.MedicalProcedures.AddAsync(
                procedure);

            await _context.SaveChangesAsync();

            return procedure;
        }

        // Finds the PatientId associated with the specified UserId.
        // Returns null if no patient is linked to the user.
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