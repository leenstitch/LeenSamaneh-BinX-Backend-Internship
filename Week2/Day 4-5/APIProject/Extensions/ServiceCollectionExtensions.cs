// This file contains extension methods used to register application services
// inside the built-in Dependency Injection container.

using APIProject.Interfaces;
using APIProject.Services;

namespace APIProject.Extensions
{
    // Static class because extension methods must be inside a static class
    public static class ServiceCollectionExtensions
    {
        // Extension method for IServiceCollection
        // It allows us to register our custom services in Program.cs easily.
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {

            // Register IBookService with BookService implementation.
            // Scoped lifetime means:
            // A new instance is created for each HTTP request.
            services.AddScoped<IBookService, BookService>();


            return services; // Return the services collection after registration.
        }
    }
}