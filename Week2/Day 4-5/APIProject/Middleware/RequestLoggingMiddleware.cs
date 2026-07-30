namespace APIProject.Middleware
{
    public class RequestLoggingMiddleware
    {
        // Holds the next middleware in the pipeline
        private readonly RequestDelegate _next;


        // Constructor receives the next middleware
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        // This method runs every time a request comes to the API
        public async Task Invoke(HttpContext context)
        {
           
            // Get HTTP method (GET, POST, PUT, DELETE)
            var method = context.Request.Method;


            // Get requested URL path (/api/books)
            var path = context.Request.Path;


            // Print request information in console
            Console.WriteLine(
                $"Request Method: {method}, Request Path: {path}"
            );


            // Continue to the next middleware/controller
            await _next(context);

        }
    }
}