using System.Security.Claims;
using LensBook.DATA;
using LensBook.Dto_s.BookingDto_s;
using LensBook.Dto_s.PhotographerDto_s;
using LensBook.Models;
using LensBook.Repository_s.IRepository;
using LensBook.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        // Get bookings for the authenticated photographer
        public async Task<IEnumerable<BookingResponseDto>>
            GetMyBookingsAsyncByUsingProjection(int userId)
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

            return await _context.Bookings
        .Where(b => b.PhotographerId == photographer.PhotographerId)
        .OrderBy(b => b.StartTime)
        .Select(b => new BookingResponseDto
        {
            BookingId = b.BookingId,
            CustomerId = b.CustomerId,
            PhotographerId = b.PhotographerId,
            SessionTypeId = b.SessionTypeId,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            Status = b.Status.ToString(),
            Notes = b.Notes
        })
        .ToListAsync();
        }

        // Get photographer details with collections
        public async Task<PhotographerDetailsDto?>
        GetDetailsWithCollectionsAsync(int photographerId)
        {
            var photographer =
                await _photographerRepository
                    .GetDetailsWithCollectionsAsync(photographerId);

            if (photographer == null)
            {
                return null;
            }

            return new PhotographerDetailsDto
            {
                PhotographerId = photographer.PhotographerId,
                FirstName = photographer.FirstName,
                LastName = photographer.LastName,
                PhoneNumber = photographer.PhoneNumber,
                Bio = photographer.Bio,

                Bookings = photographer.Bookings
                    .Select(b => new BookingDetailsDto
                    {
                        BookingId = b.BookingId,
                        CustomerId = b.CustomerId,
                        SessionTypeId = b.SessionTypeId,
                        StartTime = b.StartTime,
                        EndTime = b.EndTime,
                        Status = b.Status.ToString(),
                        Notes = b.Notes
                    })
                    .ToList(),

                ExternalSchedules = photographer.ExternalSchedules
                    .Select(e => new ExternalScheduleDto
                    {
                        ExternalScheduleId = e.ExternalScheduleId,
                        StartTime = e.StartTime,
                        EndTime = e.EndTime,
                        Location = e.Location,
                        Notes = e.Notes
                    })
                    .ToList()
            };
        }
    }
}