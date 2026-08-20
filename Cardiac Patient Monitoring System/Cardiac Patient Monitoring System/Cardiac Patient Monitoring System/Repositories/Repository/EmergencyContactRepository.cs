// This repository handles database operations for emergency contacts.
// It provides methods for retrieving, creating, updating, deleting,
// and linking emergency contacts to patients.

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

        // Retrieves an emergency contact by its ID.
        public async Task<EmergencyContact?> GetByIdAsync(int id)
        {
            return await _context.EmergencyContacts
                .Include(e => e.Patient)
                .FirstOrDefaultAsync(
                    e => e.EmergencyContactId == id);
        }

        // Retrieves emergency contacts belonging to a specific patient.
        public async Task<IEnumerable<EmergencyContact>>
            GetByPatientIdAsync(int patientId)
        {
            return await _context.EmergencyContacts
                .Where(e => e.PatientId == patientId)
                .OrderByDescending(e => e.IsPrimary)
                .ToListAsync();
        }

        // Retrieves all emergency contacts.
        public async Task<IEnumerable<EmergencyContact>>
            GetAllAsync()
        {
            return await _context.EmergencyContacts
                .Include(e => e.Patient)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        // Adds a new emergency contact and saves it to the database.
        public async Task<EmergencyContact> AddAsync(
            EmergencyContact emergencyContact)
        {
            await _context.EmergencyContacts.AddAsync(
                emergencyContact);

            await _context.SaveChangesAsync();

            return emergencyContact;
        }

        // Updates an existing emergency contact and saves the changes.
        public async Task UpdateAsync(
            EmergencyContact emergencyContact)
        {
            _context.EmergencyContacts.Update(
                emergencyContact);

            await _context.SaveChangesAsync();
        }

        // Deletes an existing emergency contact and saves the changes.
        public async Task DeleteAsync(
            EmergencyContact emergencyContact)
        {
            _context.EmergencyContacts.Remove(
                emergencyContact);

            await _context.SaveChangesAsync();
        }

        // Retrieves emergency contacts belonging to the authenticated user.
        public async Task<IEnumerable<EmergencyContact>>
            GetByUserIdAsync(int userId)
        {
            return await _context.EmergencyContacts
                .Include(e => e.Patient)
                .Where(e => e.Patient.UserId == userId)
                .OrderByDescending(e => e.IsPrimary)
                .ToListAsync();
        }

        // Finds the PatientId associated with the specified UserId.
        public async Task<int?> GetPatientIdByUserIdAsync(int userId)
        {
            return await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.PatientId)
                .FirstOrDefaultAsync();
        }
    }
}