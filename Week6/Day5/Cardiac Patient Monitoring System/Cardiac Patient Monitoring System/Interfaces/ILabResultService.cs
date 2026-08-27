using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.DTO_S.LabResultDto_s;

namespace Cardiac_Patient_Monitoring_System.Services.Interfaces
{
    public interface ILabResultService
    {

        // Returns lab results recorded within the specified date range
        // for a specific patient.
        Task<IEnumerable<LabResultResponseDto>>
            GetByPatientAndDateRangeAsync(
                int patientId,
                DateTime startDate,
                DateTime endDate);


        // Creates a new lab result for the authenticated patient.
        Task<LabResultResponseDto>
     CreateAsync(
         int userId,
         CreateLabResultDto dto);


    }
}