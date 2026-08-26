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
    }
}