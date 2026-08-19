using Cardiac_Patient_Monitoring_System.DTO_S.AppointmentDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IAppointmentService
    {
        Task<AppointmentDto?> GetByIdAsync(int id);

        Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<AppointmentDto>> GetAllAsync();

        Task<Appointment?> CreateAsync(
            int userId,
            CreateAppointmentDto dto);

        Task<IEnumerable<AppointmentDto>> GetMyAppointmentsAsync(
            int userId);

        Task<IEnumerable<AppointmentDto>>
    FilterMyAppointmentsAsync(
        int userId,
        AppointmentFilterDto filter);

        Task<IEnumerable<AppointmentDto>>
            FilterAllAsync(
                AppointmentFilterDto filter);

        Task<bool> UpdateAsync(
            int id,
            int userId,
            UpdateAppointmentDto dto);

        Task<bool> UpdateStatusAsync(
            int id,
            int userId,
            UpdateAppointmentStatusDto dto);
    }
}