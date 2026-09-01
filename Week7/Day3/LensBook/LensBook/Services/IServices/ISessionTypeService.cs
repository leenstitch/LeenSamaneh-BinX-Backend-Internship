using LensBook.Dto_s.SessionType;
using LensBook.Dto_s.SessionTypeDto_s;

namespace LensBook.Services.Interfaces
{
    public interface ISessionTypeService
    {
        Task<SessionTypeResponseDto> CreateAsync(
              CreateSessionTypeDto dto);

        Task<SessionTypeResponseDto?> GetByIdAsync(
            int id);

        Task<IEnumerable<SessionTypeResponseDto>> GetAllAsync();
    }
}