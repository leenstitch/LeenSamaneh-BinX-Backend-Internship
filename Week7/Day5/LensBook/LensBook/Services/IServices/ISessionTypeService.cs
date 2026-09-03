using LensBook.Dto_s.SessionType;
using LensBook.Dto_s.SessionTypeDto_s;

namespace LensBook.Services.Interfaces
{
    public interface ISessionTypeService
    {
        //create session type
        Task<SessionTypeResponseDto> CreateAsync(
              CreateSessionTypeDto dto);

        //get session type by id
        Task<SessionTypeResponseDto?> GetByIdAsync(
            int id);

        //get all session types
        Task<IEnumerable<SessionTypeResponseDto>> GetAllAsync();
    }
}