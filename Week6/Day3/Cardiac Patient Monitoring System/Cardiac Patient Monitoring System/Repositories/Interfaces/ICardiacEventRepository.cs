using Cardiac_Patient_Monitoring_System.DTO_S.CardiacEvent_s;

namespace Cardiac_Patient_Monitoring_System.Services.Interfaces
{
    public interface ICardiacEventRepository
    {
        Task<CardiacEventAnalysisResponseDto?>
            AnalyzeEventAsync(
                int userId,
                int cardiacEventId,
                int daysBefore);

        Task<(IEnumerable<CardiacEventVitalDto> Data, int TotalCount)>
            GetEventVitalsAsync(
                int userId,
                int cardiacEventId,
                DateTime startDate,
                DateTime endDate,
                CardiacEventVitalQueryDto query);
    }
}