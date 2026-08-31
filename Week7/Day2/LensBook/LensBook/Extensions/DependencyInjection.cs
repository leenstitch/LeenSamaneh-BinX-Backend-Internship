using LensBook.Repositories;
using LensBook.Repository_s.IRepository;
using LensBook.Services;
using LensBook.Services.Interfaces;

namespace LensBook.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}