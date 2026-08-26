using Cardiac_Patient_Monitoring_System.DTO_S.HospitalizationDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Services.Interfaces
{
    public interface IHospitalizationService
    {

        // Returns hospitalizations that overlap with the specified period.
        Task<IEnumerable<HospitalizationResponseDto>>
            GetOverlappingPeriodAsync(
                int patientId,
                DateTime startDate,
                DateTime eventDate);

        // Creates a new hospitalization record for the authenticated patient.
        Task<HospitalizationResponseDto?>
           CreateAsync(
               int userId,
               CreateHospitalizationDto dto);
    }
}