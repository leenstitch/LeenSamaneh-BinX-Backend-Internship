// This repository handles database operations for appointments.
// It provides methods for retrieving, creating, updating, filtering,
// and linking appointments to patients.

using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieves an appointment by its ID.
        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(
                    a => a.AppointmentId == id);
        }

        // Retrieves all appointments for a specific patient.
        public async Task<IEnumerable<Appointment>>
            GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        // Retrieves all appointments.
        public async Task<IEnumerable<Appointment>>
            GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        // Adds a new appointment and saves it to the database.
        public async Task<Appointment> AddAsync(
            Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();

            return appointment;
        }

        // Updates an existing appointment and saves the changes.
        public async Task UpdateAsync(
            Appointment appointment)
        {
            _context.Appointments.Update(appointment);

            await _context.SaveChangesAsync();
        }

        // Filters appointments based on patient, date, year,
        // month, and status.
        public async Task<IEnumerable<Appointment>>
            FilterAsync(
                int? patientId,
                DateTime? date,
                int? year,
                int? month,
                string? status)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .AsQueryable();

            // Filter by patient
            if (patientId.HasValue)
            {
                query = query.Where(
                    a => a.PatientId == patientId.Value);
            }

            // Filter by exact date
            if (date.HasValue)
            {
                var startDate = date.Value.Date;
                var endDate = startDate.AddDays(1);

                query = query.Where(a =>
                    a.AppointmentDate >= startDate &&
                    a.AppointmentDate < endDate);
            }

            // Filter by year
            if (year.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate.Year == year.Value);
            }

            // Filter by month
            if (month.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate.Month == month.Value);
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (Enum.TryParse<Appointment.AppointmentStatus>(
                    status,
                    true,
                    out var appointmentStatus))
                {
                    query = query.Where(
                        a => a.Status == appointmentStatus);
                }
            }

            return await query
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        // Finds the PatientId associated with the specified UserId.
        public async Task<int?> GetPatientIdByUserIdAsync(
            int userId)
        {
            return await _context.Patients
                .Where(p => p.UserId == userId)
                .Select(p => (int?)p.PatientId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasConflictAsync(
           DateTime appointmentDate)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.AppointmentDate == appointmentDate &&
                    a.Status != Appointment.AppointmentStatus.Cancelled);
        }
        public async Task<Appointment> CreateAsync(
          Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);

            return appointment;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}