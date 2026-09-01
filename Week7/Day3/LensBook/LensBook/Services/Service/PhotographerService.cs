using System.Security.Claims;
using LensBook.DATA;
using LensBook.Dto_s.BookingDto_s;
using LensBook.Dto_s.PhotographerDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace LensBook.Services
{
    public class PhotographerService : IPhotographerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPhotographerRepository _photographerRepository;
        private readonly ApplicationDbContext _context;

        public PhotographerService(
            UserManager<ApplicationUser> userManager,
            IPhotographerRepository photographerRepository,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _photographerRepository = photographerRepository;
            _context = context;
        }

        public async Task<PhotographerResponseDto> CreateAsync(
            CreatePhotographerDto dto)
        {
            // =====================================================
            // Start Transaction
            // =====================================================

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // =====================================================
                // 1. Check if email already exists
                // =====================================================

                var existingUser =
                    await _userManager.FindByEmailAsync(
                        dto.Email);

                if (existingUser != null)
                {
                    throw new Exception(
                        "A user with this email already exists.");
                }


                // =====================================================
                // 2. Create Identity User
                // =====================================================

                var user = new ApplicationUser
                {
                    UserName = dto.Email,

                    Email = dto.Email,

                    PhoneNumber = dto.PhoneNumber
                };

                var result =
                    await _userManager.CreateAsync(
                        user,
                        dto.Password);

                if (!result.Succeeded)
                {
                    var errors =
                        string.Join(
                            ", ",
                            result.Errors.Select(
                                e => e.Description));

                    throw new Exception(
                        $"Failed to create photographer account: {errors}");
                }


                // =====================================================
                // 3. Assign Photographer Role
                // =====================================================

                var roleResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        "Photographer");

                if (!roleResult.Succeeded)
                {
                    var errors =
                        string.Join(
                            ", ",
                            roleResult.Errors.Select(
                                e => e.Description));

                    throw new Exception(
                        $"Failed to assign Photographer role: {errors}");
                }


                // =====================================================
                // 4. Create Photographer
                // =====================================================

                var photographer = new Photographer
                {
                    UserId = user.Id,

                    FirstName =
                        dto.FirstName,

                    LastName =
                        dto.LastName,

                    PhoneNumber =
                        dto.PhoneNumber,

                    Bio =
                        dto.Bio,

                };


                // =====================================================
                // 5. Add Photographer
                // =====================================================

                await _photographerRepository
                    .AddAsync(photographer);


                // =====================================================
                // 6. Save Changes
                // =====================================================

                await _photographerRepository
                    .SaveChangesAsync();


                // =====================================================
                // 7. Commit Transaction
                // =====================================================

                await transaction.CommitAsync();


                // =====================================================
                // 8. Return Created Photographer
                // =====================================================

                return new PhotographerResponseDto
                {
                    PhotographerId =
                        photographer.PhotographerId,

                    UserId =
                        photographer.UserId,

                    FirstName =
                        photographer.FirstName,

                    LastName =
                        photographer.LastName,

                    PhoneNumber =
                        photographer.PhoneNumber,

                    Bio =
                        photographer.Bio,

                   
                };
            }
            catch
            {
                // =====================================================
                // Rollback if anything fails
                // =====================================================

                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<IEnumerable<BookingResponseDto>>
    GetMyBookingsAsync(int userId)
        {
            // =====================================================
            // 1. Get photographer linked to logged-in user
            // =====================================================

            var photographer =
                await _photographerRepository
                    .GetByUserIdAsync(userId);

            if (photographer == null)
            {
                throw new Exception(
                    "Photographer profile not found.");
            }


            // =====================================================
            // 2. Get photographer's bookings
            // =====================================================

            var bookings =
                await _photographerRepository
                    .GetMyBookingsAsync(
                        photographer.PhotographerId);


            // =====================================================
            // 3. Map to DTO
            // =====================================================

            return bookings.Select(
                booking =>
                    new BookingResponseDto
                    {
                        BookingId =
                            booking.BookingId,

                        CustomerId =
                            booking.CustomerId,

                        PhotographerId =
                            booking.PhotographerId,

                        SessionTypeId =
                            booking.SessionTypeId,

                        StartTime =
                            booking.StartTime,

                        EndTime =
                            booking.EndTime,

                        Status = booking.Status.ToString(),


                        Notes =
                            booking.Notes
                    });
        }
    }
}