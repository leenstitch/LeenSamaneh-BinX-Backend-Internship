using Cardiac_Patient_Monitoring_System.DTO_S.MedicalTimelineItemDto_s;

namespace Cardiac_Patient_Monitoring_System.Interfaces
{
    public interface IMedicalTimelineService
    {
        Task<MedicalTimelineResponseDto> GetPatientMedicalTimelineAsync(
            int patientId,
            int page,
            int pageSize);
    }
}
