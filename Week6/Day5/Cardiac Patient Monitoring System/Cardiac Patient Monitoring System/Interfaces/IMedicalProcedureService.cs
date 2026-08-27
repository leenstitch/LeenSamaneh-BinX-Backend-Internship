using Cardiac_Patient_Monitoring_System.DTO_S.MedicalProcedureDto_s;

namespace Cardiac_Patient_Monitoring_System.Services.Interfaces
{
    public interface IMedicalProcedureService
    {

        // Returns medical procedures performed within the specified date range
        // for a specific patient.
        Task<IEnumerable<MedicalProcedureResponseDto>>
            GetByPatientAndDateRangeAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate);

        // Creates a new medical procedure for the authenticated patient.
        Task<MedicalProcedureResponseDto?>
            CreateAsync(
                int userId,
                CreateMedicalProcedureDto dto);
    }
}