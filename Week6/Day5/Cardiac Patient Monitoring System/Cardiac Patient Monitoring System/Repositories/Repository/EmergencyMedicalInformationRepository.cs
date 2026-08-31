using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories.Repository
{
    public class EmergencyMedicalInformationRepository : IEmergencyMedicalInformationRepository
    {
        private readonly ApplicationDbContext _context;

        public EmergencyMedicalInformationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmergencyMedicalInformation?>
             GetByPatientIdAsync(int patientId)
        {
            return await _context.EmergencyMedicalInformation
                .FirstOrDefaultAsync(e =>
                    e.PatientId == patientId);
        }
    }
}
