using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s;
using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEventAnalysisDto_s;
using Cardiac_Patient_Monitoring_System.Models;

namespace Cardiac_Patient_Monitoring_System.Services.Interfaces
{
    public interface ICardiacEventAnalysisService
    {

        // Analyzes a cardiac event using the patient's medical data
        // recorded during the specified period before the event.
        Task<CardiacEventAnalysisResponseDto?>
            AnalyzeEventAsync(
                int userId,
                int cardiacEventId,
                int daysBefore);


        // Creates a new cardiac event for the authenticated patient.
        Task<CardiacEventResponseDto?>
          CreateAsync(
              int userId,
              CreateCardiacEventDto dto);

    ////////    // Returns the latest vital-sign record recorded before
    ////////    // the specified cardiac event.
    ////////    Task<VitalSign?> GetLatestVitalBeforeEventAsync(
    ////////int userId,
    ////////int cardiacEventId);
    }


}