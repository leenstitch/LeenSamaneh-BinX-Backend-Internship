// This interface defines the operations provided by the Appointment service.
// It handles retrieving, creating, filtering, and updating appointments.

using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IAppointmentService
    {
        // Returns an appointment by its ID.
        Task<AppointmentDto?> GetByIdAsync(int id);

        // Returns all appointments belonging to a specific patient.
        Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId);

        // Returns all appointments.
        Task<IEnumerable<AppointmentDto>> GetAllAsync();

        // Creates a new appointment for the patient linked to the user.
        Task<Appointment?> CreateAsync(
            int userId,
            CreateAppointmentDto dto);

        // Returns appointments belonging to the authenticated patient.
        Task<IEnumerable<AppointmentDto>> GetMyAppointmentsAsync(
            int userId);

        // Filters appointments for the authenticated patient.
        Task<IEnumerable<AppointmentDto>>
            FilterMyAppointmentsAsync(
                int userId,
                AppointmentFilterDto filter);

        // Filters all appointments.
        Task<IEnumerable<AppointmentDto>>
            FilterAllAsync( AppointmentFilterDto filter);

        // Updates an existing appointment for the authenticated patient.
        Task<bool> UpdateAsync(
            int id,
            int userId,
            UpdateAppointmentDto dto);

        // Updates only the status of an appointment.
        Task<bool> UpdateStatusAsync(
            int id,
            int userId,
            UpdateAppointmentStatusDto dto);
    }
}