// This middleware handles unexpected exceptions across the application.
// It logs unhandled errors and returns a consistent HTTP 500 response.

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace Cardiac_Patient_Monitoring_System.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Executes the next component in the request pipeline
        // and handles any unhandled exception that occurs.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Invalid request input for {RequestPath}",
                    context.Request.Path
                );

                context.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                context.Response.ContentType =
                    "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Title = "Invalid request.",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred while processing {RequestPath}",
                    context.Request.Path
                );

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType =
                    "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }


        }
    }
}