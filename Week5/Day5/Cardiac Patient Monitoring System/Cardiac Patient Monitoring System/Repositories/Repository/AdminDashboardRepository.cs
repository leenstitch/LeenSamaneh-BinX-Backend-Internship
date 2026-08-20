using Cardiac_Patient_Monitoring_System.Data;
using Cardiac_Patient_Monitoring_System.DTO_S.AdminDashboardDto_s;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cardiac_Patient_Monitoring_System.Repositories
{
    public class AdminDashboardRepository
        : IAdminDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardOverviewDto>
            GetOverviewAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var totalPatients =
                await _context.Patients.CountAsync();

            var totalVitalSigns =
                await _context.VitalSigns.CountAsync();

            var totalMedications =
                await _context.Medications.CountAsync();

            var totalDiagnoses =
                await _context.Diagnoses.CountAsync();

            var totalAppointments =
                await _context.Appointments.CountAsync();

            var scheduledAppointments =
                await _context.Appointments.CountAsync(
                    a => a.Status ==
                         Appointment.AppointmentStatus.Scheduled);

            var completedAppointments =
                await _context.Appointments.CountAsync(
                    a => a.Status ==
                         Appointment.AppointmentStatus.Completed);

            var cancelledAppointments =
                await _context.Appointments.CountAsync(
                    a => a.Status ==
                         Appointment.AppointmentStatus.Cancelled);

            var appointmentsToday =
                await _context.Appointments.CountAsync(
                    a =>
                        a.AppointmentDate >= today &&
                        a.AppointmentDate < tomorrow);

            var latestVitalSignIds =
    await _context.VitalSigns
        .GroupBy(v => v.PatientId)
        .Select(g => g.Max(v => v.VitalSignId))
        .ToListAsync();

            var patientsNeedingAttention =
                await _context.VitalSigns
                    .Where(v =>
                        latestVitalSignIds.Contains(v.VitalSignId))
                    .CountAsync(v =>
                        v.HeartRate > 100 ||
                        v.HeartRate < 60 ||
                        v.SystolicPressure >= 140 ||
                        v.DiastolicPressure >= 90 ||
                        v.OxygenSaturation < 92 ||
                        v.Temperature >= 38);

            return new AdminDashboardOverviewDto
            {
                TotalPatients = totalPatients,
                TotalVitalSigns = totalVitalSigns,
                TotalMedications = totalMedications,
                TotalDiagnoses = totalDiagnoses,
                TotalAppointments = totalAppointments,
                ScheduledAppointments =
                    scheduledAppointments,
                CompletedAppointments =
                    completedAppointments,
                CancelledAppointments =
                    cancelledAppointments,
                AppointmentsToday =
                    appointmentsToday,
                PatientsNeedingAttention =
                    patientsNeedingAttention
            };
        }
        public async Task<IEnumerable<PatientAtRiskDto>>
    GetPatientsAtRiskAsync()
        {
            var latestVitalSigns = await _context.VitalSigns
                .Include(v => v.Patient)
                .GroupBy(v => v.PatientId)
                .Select(g => g
                    .OrderByDescending(v => v.VitalSignId)
                    .First())
                .ToListAsync();

            var patientsAtRisk = new List<PatientAtRiskDto>();

            foreach (var vitalSign in latestVitalSigns)
            {
                var alerts = new List<string>();

                if (vitalSign.HeartRate > 100)
                    alerts.Add("High heart rate.");

                if (vitalSign.HeartRate < 60)
                    alerts.Add("Low heart rate.");

                if (vitalSign.SystolicPressure >= 140)
                    alerts.Add("High systolic pressure.");

                if (vitalSign.DiastolicPressure >= 90)
                    alerts.Add("High diastolic pressure.");

                if (vitalSign.OxygenSaturation < 92)
                    alerts.Add("Low oxygen saturation.");

                if (vitalSign.Temperature >= 38)
                    alerts.Add("Elevated temperature.");

                if (alerts.Count > 0)
                {
                    patientsAtRisk.Add(new PatientAtRiskDto
                    {
                        PatientId = vitalSign.PatientId,

                        PatientName =
                            $"{vitalSign.Patient.FirstName} " +
                            $"{vitalSign.Patient.LastName}",

                        Alerts = alerts
                    });
                }
            }

            return patientsAtRisk;
        }
    }
}