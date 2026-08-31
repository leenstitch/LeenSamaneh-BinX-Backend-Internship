using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories.Repository
{
    public class FamilyMedicalHistoryRepository : IFamilyMedicalHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public FamilyMedicalHistoryRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FamilyMedicalHistory>> GetByPatientIdAsync(
      int patientId)
        {
            return await _context.FamilyMedicalHistories
                .Where(f => f.PatientId == patientId)
                .ToListAsync();
        }

        public async Task CreateRangeAsync(
     List<FamilyMedicalHistory> histories)
        {
            await _context.FamilyMedicalHistories
                .AddRangeAsync(histories);
        }
    }
}
