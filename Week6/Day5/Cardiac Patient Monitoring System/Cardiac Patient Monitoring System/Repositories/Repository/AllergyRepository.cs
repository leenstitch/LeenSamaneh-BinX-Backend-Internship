using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories.Repository
{
    public class AllergyRepository : IAllergyRepository
    {
        private readonly ApplicationDbContext _context;

        public AllergyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Allergy>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Allergies
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task CreateRangeAsync(
           List<Allergy> allergies)
        {
            await _context.Allergies
                .AddRangeAsync(allergies);
        }

    }
    }