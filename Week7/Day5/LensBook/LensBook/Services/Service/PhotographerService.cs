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

        //create a new photographer account
        public async Task<PhotographerResponseDto> CreateAsync(
            CreatePhotographerDto dto)
        {
           

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                

                var existingUser =
                    await _userManager.FindByEmailAsync(
                        dto.Email);

                if (existingUser != null)
                {
                    throw new ArgumentException(
                        "A user with this email already exists.");
                }



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

                    throw new ArgumentException(
                        $"Failed to create photographer account: {errors}");
                }



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

                    throw new KeyNotFoundException(
                        $"Failed to assign Photographer role: {errors}");
                }



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


                await _photographerRepository
                    .AddAsync(photographer);



                await _photographerRepository
                    .SaveChangesAsync();



                await transaction.CommitAsync();



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
                

                await transaction.RollbackAsync();

                throw;
            }
        }

        // get a photographer bookings
        public async Task<IEnumerable<BookingResponseDto>>
    GetMyBookingsAsync(int userId)
        {
           

            var photographer =
                await _photographerRepository
                    .GetByUserIdAsync(userId);

            if (photographer == null)
            {
                throw new Exception(
                    "Photographer profile not found.");
            }



            var bookings =
                await _photographerRepository
                    .GetMyBookingsAsync(
                        photographer.PhotographerId);


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