// This interface defines the database operations for the Appointment repository.
// It handles retrieving, creating, updating, filtering, and linking appointments to patients.

using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        // Returns an appointment by its ID.
        Task<Appointment?> GetByIdAsync(int id);

        // Returns appointments belonging to a specific patient.
        Task<IEnumerable<Appointment>> GetByPatientIdAsync(
            int patientId);

        // Returns all appointments.
        Task<IEnumerable<Appointment>> GetAllAsync();

        // Creates a new appointment.
        Task<Appointment> AddAsync(
            Appointment appointment);

        // Updates an existing appointment.
        Task UpdateAsync(
            Appointment appointment);

        // Filters appointments based on the provided criteria.
        Task<IEnumerable<Appointment>> FilterAsync(
            int? patientId,
            DateTime? date,
            int? year,
            int? month,
            string? status);

        // Finds the PatientId linked to the specified user.
        Task<int?> GetPatientIdByUserIdAsync(int userId);


        Task<bool> HasConflictAsync(DateTime appointmentDate);

        Task<Appointment> CreateAsync(Appointment appointment);

        Task SaveChangesAsync();

    }
}