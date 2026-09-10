using LensBook.Models;

namespace LensBook.Repository_s.IRepository
{
    public interface IExternalScheduleRepository
    {

        //check if a photographer has overlapping external schedules
        Task<bool> HasOverlappingScheduleAsync(
            int photographerId,
            DateTime startTime,
            DateTime endTime);


        // Add a new external schedule
        Task<ExternalSchedule> AddAsync(
            ExternalSchedule externalSchedule);
    }
}