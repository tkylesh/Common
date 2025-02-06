using System.Diagnostics;
using System.Text;

namespace CommonAPI.Middleware
{
    public class ApiLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggerMiddleware> _logger;

        public ApiLoggerMiddleware(RequestDelegate next, ILogger<ApiLoggerMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = Guid.NewGuid();
            var stopwatch = Stopwatch.StartNew();

            if (context.Request.Path.ToString().ToLower().Contains("swagger") || context.Request.Path.ToString().ToLower().Contains("health"))
            {
                await _next(context);
                return;
            }

            await this.LogRequest(context, correlationId);
            await this.LogResponse(context, correlationId, stopwatch);
        }

        private async Task LogRequest(HttpContext context, Guid correlationId)
        {
            var logString = new StringBuilder();

            try
            {
                var request = context.Request;

                logString.AppendLine($"Request Date: {DateTime.Now}");
                logString.AppendLine($"URL: {request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
                logString.AppendLine($"correlationId: {correlationId}");

                var headers = new StringBuilder();
                foreach (var header in request.Headers)
                {
                    headers.AppendLine($"{header.Key}: {header.Value}");
                }
                logString.AppendLine("Headers:\n" + headers.ToString());

                request.EnableBuffering();

                var requestBody = await new StreamReader(request.Body, Encoding.UTF8).ReadToEndAsync();

                logString.AppendLine($"Request Body: \n{requestBody}");

                _logger.LogInformation(logString.ToString());

                context.Request.Body.Position = 0;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "request logging failed");
            }
        }

        private async Task LogResponse(HttpContext context, Guid correlationId, Stopwatch stopwatch)
        {
            stopwatch.Stop();
            var request = context.Request;
            var response = context.Response;
            var logString = new StringBuilder();

            try
            {
                var originalBodyStream = context.Response.Body;

                logString.AppendLine($"Response Date: {DateTime.Now}");
                logString.AppendLine($"URL: {request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
                logString.AppendLine($"correlationId: {correlationId}");
                logString.AppendLine($"duration: {stopwatch.Elapsed}");

                using var memStream = new MemoryStream();
                response.Body = memStream;

                await _next(context);

                logString.AppendLine($"Status Code: {response.StatusCode}");
                memStream.Position = 0;
                var responseBody = new StreamReader(memStream).ReadToEnd();
                logString.AppendLine($"Response Body: \n {responseBody}");
                _logger.LogInformation(logString.ToString());

                response.Body.Position = 0;

                await memStream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"response logging failed\ncorrelationId{correlationId}");
            }
        }
    }
}
