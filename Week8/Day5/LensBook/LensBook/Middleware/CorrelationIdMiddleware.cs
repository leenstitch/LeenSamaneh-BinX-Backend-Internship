namespace LensBook.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Add a correlation ID to the response headers if it doesn't exist in the request headers
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId =
                context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }

            context.Response.Headers["X-Correlation-ID"] =
                correlationId;

            await _next(context);
        }
    }
}