using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int id);

        Task<IEnumerable<Appointment>> GetByPatientIdAsync(
            int patientId);

        Task<IEnumerable<Appointment>> GetAllAsync();

        Task<Appointment> AddAsync(
            Appointment appointment);

        Task UpdateAsync(
            Appointment appointment);

        Task<IEnumerable<Appointment>> FilterAsync(
            int? patientId,
            DateTime? date,
            int? year,
            int? month,
            string? status);

        Task<int?> GetPatientIdByUserIdAsync(int userId);
    }
}