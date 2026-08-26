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


        // Retrieves hospitalizations that overlap with
        // the specified cardiac event analysis period.
        // Includes ongoing hospitalizations with no discharge date.
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


        // Creates a new hospitalization record
        // and saves it to the database.
        public async Task<Hospitalization> AddAsync(
        Hospitalization hospitalization)
        {
            await _context.Hospitalizations.AddAsync(
                hospitalization);

            await _context.SaveChangesAsync();

            return hospitalization;
        }


        // Finds the PatientId associated with
        // the specified UserId.
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