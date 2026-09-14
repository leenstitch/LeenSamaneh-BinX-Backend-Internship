using System.Security.Claims;
using LensBook.DATA;
using LensBook.Dto_s.ExternalScheduleDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LensBook.Services
{
    public class ExternalScheduleService
        : IExternalScheduleService
    {
        private readonly IExternalScheduleRepository
            _externalScheduleRepository;

        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor
            _httpContextAccessor;

        public ExternalScheduleService(
            IExternalScheduleRepository externalScheduleRepository,
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _externalScheduleRepository =
                externalScheduleRepository;

            _context = context;

            _httpContextAccessor =
                httpContextAccessor;
        }

        public async Task<ExternalScheduleResponseDto>
            AddExternalScheduleAsync(
                CreateExternalScheduleDto dto)
        {
            // =========================================
            // 1. Get UserId from JWT Token
            // =========================================

            var userIdClaim =
                _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue(
                        ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException(
                    "User ID was not found in the token.");
            }

            if (!int.TryParse(
                    userIdClaim,
                    out int userId))
            {
                throw new UnauthorizedAccessException(
                    "Invalid User ID in token.");
            }


            // =========================================
            // 2. Get Photographer using UserId
            // =========================================

            var photographer =
                await _context.Photographers
                    .FirstOrDefaultAsync(
                        p => p.UserId == userId);

            if (photographer == null)
            {
                throw new UnauthorizedAccessException(
                    "The current user is not a photographer.");
            }

            int photographerId =
                photographer.PhotographerId;


            // =========================================
            // 3. Validate time
            // =========================================

            if (dto.StartTime >= dto.EndTime)
            {
                throw new ArgumentException(
                    "Start time must be before end time.");
            }


            // =========================================
            // 4. Check overlapping schedules
            // =========================================

            var hasOverlap =
                await _externalScheduleRepository
                    .HasOverlappingScheduleAsync(
                        photographerId,
                        dto.StartTime,
                        dto.EndTime);

            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "The photographer already has an external booking during this time.");
            }


            // =========================================
            // 5. Create ExternalSchedule
            // =========================================

            var externalSchedule =
                new ExternalSchedule
                {
                    PhotographerId = photographerId,

                    StartTime = dto.StartTime,

                    EndTime = dto.EndTime,

                    Location = dto.Location,

                    Notes = dto.Notes
                };


            // =========================================
            // 6. Save to database
            // =========================================

            var createdSchedule =
                await _externalScheduleRepository
                    .AddAsync(externalSchedule);


            // =========================================
            // 7. Return Response DTO
            // =========================================

            return new ExternalScheduleResponseDto
            {
                ExternalScheduleId =
                    createdSchedule.ExternalScheduleId,

                PhotographerId =
                    createdSchedule.PhotographerId,

                StartTime =
                    createdSchedule.StartTime,

                EndTime =
                    createdSchedule.EndTime,

                Location =
                    createdSchedule.Location,

                Notes =
                    createdSchedule.Notes
            };
        }
    }
}