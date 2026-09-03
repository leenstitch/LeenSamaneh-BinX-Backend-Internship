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


        // CREATE session type

        public async Task<SessionTypeResponseDto> CreateAsync(
            CreateSessionTypeDto dto)
        {

            if (dto.DurationInMinutes <= 0)
            {
                throw new ArgumentException(
                    "Duration must be greater than zero.");
            }

            if (dto.DurationInMinutes > 1440)
            {
                throw new ArgumentException(
                    "Duration cannot exceed 1440 minutes.");
            }

            if (dto.Price < 0)
            {
                throw new ArgumentException(
                    "Price cannot be negative.");
            }
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


        // GET ALL session types

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