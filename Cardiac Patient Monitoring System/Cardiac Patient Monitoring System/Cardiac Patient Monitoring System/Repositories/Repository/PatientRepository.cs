// This repository handles database operations for patients.
// It uses Entity Framework Core to retrieve, update, delete,
// and load patient health data.

using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves a patient using the User ID associated with the account.
        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        // Retrieves a patient using the Patient ID.
        public async Task<Patient?> GetByIdAsync(int patientId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        // Retrieves all patients from the database.
        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .ToListAsync();
        }

        // Marks a patient as modified in Entity Framework Core.
        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
        }

        // Marks a patient for deletion.
        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
        }

        // Saves all pending database changes.
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Retrieves a patient together with their related health data.
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

        // Retrieves a patient together with their health data using User ID.
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