using System.Text;

namespace CommonAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // Log the exception and its stack trace
                _logger.LogError(ex, "Unhandled exception occurred.");

                // Respond with a generic error message to the client
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var errorMessage = "An error occurred while processing the request.";
                await context.Response.WriteAsync(errorMessage, Encoding.UTF8);
            }
        }
    }
}
