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

        public async Task<IEnumerable<LabResult>>
            GetByPatientAndDateRangeAsync(
                int patientId,
                DateTime startDate,
                DateTime endDate)
        {
            return await _context.LabResults
                .AsNoTracking()
                .Where(x =>
                    x.PatientId == patientId &&
                    x.TestDate >= startDate &&
                    x.TestDate <= endDate)
                .OrderBy(x => x.TestDate)
                .ToListAsync();
        }
    }
}