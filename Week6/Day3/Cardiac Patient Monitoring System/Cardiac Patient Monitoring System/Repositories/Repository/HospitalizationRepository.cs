using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class HospitalizationRepository
        : IHospitalizationRepository
    {
        private readonly ApplicationDbContext _context;

        public HospitalizationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hospitalization>>
            GetOverlappingPeriodAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate)
        {
            return await _context.Hospitalizations
                .AsNoTracking()
                .Where(x =>
                    x.PatientId == patientId &&
                    x.AdmissionDate <= eventDate &&
                    (
                        x.DischargeDate == null ||
                        x.DischargeDate >= startDate
                    ))
                .OrderByDescending(x => x.AdmissionDate)
                .ToListAsync();
        }
    }
}