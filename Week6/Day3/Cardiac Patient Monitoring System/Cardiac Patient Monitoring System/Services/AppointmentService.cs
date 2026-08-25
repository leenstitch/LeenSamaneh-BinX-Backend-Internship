// This service handles the business logic for appointments.
// It provides operations for retrieving, creating, filtering,
// updating, and changing appointment status.

using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s;
using Cardiac_Patient_Monitoring_System.Interfaces;
using Cardiac_Patient_Monitoring_System.Models;
using Cardiac_Patient_Monitoring_System.Repositories.Interfaces;

namespace Cardiac_Patient_Monitoring_System.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(
            IAppointmentRepository repository)
        {
            _repository = repository;
        }

        // Returns a single appointment by its ID.
        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
                return null;

            return new AppointmentDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                RecordedByDoctorName = appointment.RecordedByDoctorName,
                AppointmentDate = appointment.AppointmentDate,
                Reason = appointment.Reason,
                Status = appointment.Status.ToString(),
                Notes = appointment.Notes,
                Location = appointment.Location,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt
            };
        }

        // Returns appointments belonging to the authenticated patient.
        public async Task<IEnumerable<AppointmentDto>> GetMyAppointmentsAsync(int userId)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return Enumerable.Empty<AppointmentDto>();

            var appointments =
                await _repository.GetByPatientIdAsync(
                    patientId.Value);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Returns all appointments.
        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _repository.GetAllAsync();

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                Location = a.Location,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Creates a new appointment for the patient linked to the user.
        // New appointments are automatically created with Scheduled status.
        public async Task<Appointment?> CreateAsync(
            int userId,
            CreateAppointmentDto dto)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(
                    userId);

            if (!patientId.HasValue)
                return null;

            var appointment = new Appointment
            {
                PatientId = patientId.Value,

                AppointmentDate = dto.AppointmentDate,

                Reason = dto.Reason,

                Location = dto.Location,

                Notes = dto.Notes,

                RecordedByDoctorName =
                    dto.RecordedByDoctorName,

                Status =
                    Appointment.AppointmentStatus.Scheduled,

                CreatedAt = DateTime.UtcNow,

                UpdatedAt = DateTime.UtcNow
            };

            return await _repository.AddAsync(appointment);
        }

        // Returns all appointments belonging to a specific patient.
        public async Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            var appointments =
                await _repository.GetByPatientIdAsync(patientId);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Filters appointments belonging to the authenticated patient.
        public async Task<IEnumerable<AppointmentDto>>
            FilterMyAppointmentsAsync(
                int userId,
                AppointmentFilterDto filter)
        {
            var patientId =
                await _repository.GetPatientIdByUserIdAsync(userId);

            if (!patientId.HasValue)
                return Enumerable.Empty<AppointmentDto>();

            var appointments =
                await _repository.FilterAsync(
                    patientId.Value,
                    filter.Date,
                    filter.Year,
                    filter.Month,
                    filter.Status);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Filters appointments across all patients.
        public async Task<IEnumerable<AppointmentDto>>
            FilterAllAsync(
                AppointmentFilterDto filter)
        {
            var appointments =
                await _repository.FilterAsync(
                    null,
                    filter.Date,
                    filter.Year,
                    filter.Month,
                    filter.Status);

            return appointments.Select(a => new AppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                RecordedByDoctorName = a.RecordedByDoctorName,
                AppointmentDate = a.AppointmentDate,
                Reason = a.Reason,
                Status = a.Status.ToString(),
                Notes = a.Notes,
                CreatedAt = a.CreatedAt,
                Location = a.Location,
                UpdatedAt = a.UpdatedAt
            });
        }

        // Updates appointment information for the authenticated patient.
        public async Task<bool> UpdateAsync(
            int id,
            int userId,
            UpdateAppointmentDto dto)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
                return false;

            var patientId =
                await _repository.GetPatientIdByUserIdAsync(
                    userId);

            // Ensures that the appointment belongs to
            // the authenticated patient.
            if (!patientId.HasValue ||
                appointment.PatientId != patientId.Value)
            {
                return false;
            }

            // Only Scheduled appointments can be edited.
            if (appointment.Status !=
                Appointment.AppointmentStatus.Scheduled)
            {
                return false;
            }

            if (dto.AppointmentDate.HasValue)
            {
                appointment.AppointmentDate =
                    dto.AppointmentDate.Value;
            }

            if (dto.Reason != null)
            {
                appointment.Reason = dto.Reason;
            }

            if (dto.Location != null)
            {
                appointment.Location = dto.Location;
            }

            if (dto.Notes != null)
            {
                appointment.Notes = dto.Notes;
            }

            appointment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(appointment);

            return true;
        }

        // Updates only the status of an appointment.
        public async Task<bool> UpdateStatusAsync(
            int id,
            int userId,
            UpdateAppointmentStatusDto dto)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
                return false;

            var patientId =
                await _repository.GetPatientIdByUserIdAsync(
                    userId);

            // Ensures that the appointment belongs to
            // the authenticated patient.
            if (!patientId.HasValue ||
                appointment.PatientId != patientId.Value)
            {
                return false;
            }

            // Only Scheduled appointments can change status.
            if (appointment.Status !=
                Appointment.AppointmentStatus.Scheduled)
            {
                return false;
            }

            // Only Completed or Cancelled statuses are allowed.
            if (dto.Status !=
                    Appointment.AppointmentStatus.Completed &&
                dto.Status !=
                    Appointment.AppointmentStatus.Cancelled)
            {
                return false;
            }

            appointment.Status =
                (Appointment.AppointmentStatus)dto.Status;

            appointment.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(appointment);

            return true;
        }
    }
}