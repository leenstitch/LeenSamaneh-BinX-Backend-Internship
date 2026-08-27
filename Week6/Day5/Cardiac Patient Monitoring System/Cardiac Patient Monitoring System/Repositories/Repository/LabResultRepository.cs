using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class LabResultRepository : ILabResultRepository
    {
        private readonly ApplicationDbContext _context;

        public LabResultRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves ALL lab results for cardiac event analysis.
        // No pagination or filtering is applied here.
        public async Task<IEnumerable<LabResult>>
            GetForCardiacEventAnalysisAsync(
                int patientId,
                DateTime startDate,
                DateTime endDate)
        {
            return await _context.LabResults
                .AsNoTracking()
                .Where(x =>
                    x.PatientId == patientId &&
                    x.TestDate >= startDate &&
                    x.TestDate < endDate)
                .OrderBy(x => x.TestDate)
                .ToListAsync();
        }

        // Creates a new lab result and saves it to the database.
        public async Task<LabResult>
           AddAsync(LabResult labResult)
        {
            await _context.LabResults.AddAsync(labResult);

            await _context.SaveChangesAsync();

            return labResult;
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