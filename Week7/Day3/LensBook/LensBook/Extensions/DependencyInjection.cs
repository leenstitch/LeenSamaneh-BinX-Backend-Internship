using LensBook.Repositories;
using LensBook.Repository_s.IRepository;
using LensBook.Services;
using LensBook.Services.Interfaces;
using LensBook.Services.IServices;

namespace LensBook.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPhotographerRepository, PhotographerRepository>();
            services.AddScoped<IPhotographerService, PhotographerService>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ISessionTypeRepository, SessionTypeRepository>();
            services.AddScoped<ISessionTypeService, SessionTypeService>();
            return services;
        }
    }
}