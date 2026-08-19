using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class EmergencyContactRepository
        : IEmergencyContactRepository
    {
        private readonly ApplicationDbContext _context;

        public EmergencyContactRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EmergencyContact?> GetByIdAsync(int id)
        {
            return await _context.EmergencyContacts
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(
                    e => e.EmergencyContactId == id);
        }

        public async Task<IEnumerable<EmergencyContact>>
            GetByPatientIdAsync(int patientId)
        {
            return await _context.EmergencyContacts
                .Where(e => e.PatientId == patientId)
                .OrderByDescending(e => e.IsPrimary)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmergencyContact>>
            GetAllAsync()
        {
            return await _context.EmergencyContacts
                .Include(e => e.Patient)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<EmergencyContact> AddAsync(
            EmergencyContact emergencyContact)
        {
            await _context.EmergencyContacts.AddAsync(
                emergencyContact);

            await _context.SaveChangesAsync();

            return emergencyContact;
        }

        public async Task UpdateAsync(
            EmergencyContact emergencyContact)
        {
            _context.EmergencyContacts.Update(
                emergencyContact);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            EmergencyContact emergencyContact)
        {
            _context.EmergencyContacts.Remove(
                emergencyContact);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmergencyContact>>
            GetByUserIdAsync(int userId)
        {
            return await _context.EmergencyContacts
                .Include(e => e.Patient)
                .Where(e => e.Patient.UserId == userId)
                .OrderByDescending(e => e.IsPrimary)
                .ToListAsync();
        }
        public async Task<int?> GetPatientIdByUserIdAsync(int userId)
        {
            return await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.PatientId)
                .FirstOrDefaultAsync();
        }
    }
}