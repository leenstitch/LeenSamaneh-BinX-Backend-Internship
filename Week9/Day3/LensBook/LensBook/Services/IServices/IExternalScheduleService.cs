using LensBook.Dto_s.ExternalScheduleDto_s;


namespace LensBook.Services.Interfaces
{
    public interface IExternalScheduleService
    {
        Task<ExternalScheduleResponseDto> AddExternalScheduleAsync(
            CreateExternalScheduleDto dto);
    }
}