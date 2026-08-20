// This class implements IPatientRepository and handles patient data access.
// It uses Entity Framework Core to communicate with the SQL Server database.
// The repository keeps database operations separate from the service layer.

using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories.Repository
{
    public class PatientRepository : IPatientRepository
    {
        // The database context used to access the Patients table.
        private readonly ApplicationDbContext _context;

        // Injects the database context through dependency injection.
        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Finds a patient by the User ID linked to their account.
        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        // Finds a patient by their Patient ID.
        public async Task<Patient?> GetByIdAsync(int patientId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        // Retrieves all patient records from the database.
        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .ToListAsync();
        }

        // Marks the patient as modified in Entity Framework Core.
        // The actual database update happens when SaveChangesAsync is called.
        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
        }

        // Marks the patient for deletion.
        // The actual deletion happens when SaveChangesAsync is called.
        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
        }

        // Saves all pending changes made through the DbContext.
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Patient?> GetWithHealthDataByIdAsync(int patientId)
        {
            return await _context.Patients
                .Include(p => p.VitalSigns)
                .Include(p => p.Medications)
                .Include(p => p.Diagnoses)
                .Include(p => p.Appointments)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }
        public async Task<Patient?> GetWithHealthDataByUserIdAsync(int userId)
        {
            return await _context.Patients
                .Include(p => p.VitalSigns)
                .Include(p => p.Medications)
                .Include(p => p.Diagnoses)
                .Include(p => p.Appointments)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
    }
}