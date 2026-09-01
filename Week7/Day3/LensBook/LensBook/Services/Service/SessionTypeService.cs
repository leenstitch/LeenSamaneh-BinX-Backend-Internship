using LensBook.Dto_s.SessionType;
using LensBook.Dto_s.SessionTypeDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services.Interfaces;

namespace LensBook.Services
{
    public class SessionTypeService : ISessionTypeService
    {
        private readonly ISessionTypeRepository _repository;

        public SessionTypeService(
            ISessionTypeRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<SessionTypeResponseDto> CreateAsync(
            CreateSessionTypeDto dto)
        {
            var sessionType = new SessionType
            {
                Name = dto.Name,

                Description = dto.Description,

                DurationInMinutes =
                    dto.DurationInMinutes,

                Price = dto.Price,

                IsActive = dto.IsActive
            };

            await _repository.AddAsync(
                sessionType);

            await _repository.SaveChangesAsync();

            return new SessionTypeResponseDto
            {
                SessionTypeId =
                    sessionType.SessionTypeId,

                Name =
                    sessionType.Name,

                Description =
                    sessionType.Description,

                DurationInMinutes =
                    sessionType.DurationInMinutes,

                Price =
                    sessionType.Price,

                IsActive =
                    sessionType.IsActive
            };
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<SessionTypeResponseDto?> GetByIdAsync(
            int id)
        {
            var sessionType =
                await _repository.GetByIdAsync(id);

            if (sessionType == null)
            {
                return null;
            }

            return new SessionTypeResponseDto
            {
                SessionTypeId =
                    sessionType.SessionTypeId,

                Name =
                    sessionType.Name,

                Description =
                    sessionType.Description,

                DurationInMinutes =
                    sessionType.DurationInMinutes,

                Price =
                    sessionType.Price,

                IsActive =
                    sessionType.IsActive
            };
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<SessionTypeResponseDto>> GetAllAsync()
        {
            var sessionTypes =
                await _repository.GetAllAsync();

            return sessionTypes.Select(
                sessionType =>
                    new SessionTypeResponseDto
                    {
                        SessionTypeId =
                            sessionType.SessionTypeId,

                        Name =
                            sessionType.Name,

                        Description =
                            sessionType.Description,

                        DurationInMinutes =
                            sessionType.DurationInMinutes,

                        Price =
                            sessionType.Price,

                        IsActive =
                            sessionType.IsActive
                    });
        }
    }
}