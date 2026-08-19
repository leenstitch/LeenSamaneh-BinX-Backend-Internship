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

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(
                    a => a.AppointmentId == id);
        }

        public async Task<IEnumerable<Appointment>>
            GetByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>>
            GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment> AddAsync(
            Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task UpdateAsync(
            Appointment appointment)
        {
            _context.Appointments.Update(appointment);

            await _context.SaveChangesAsync();
        }

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