using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.ExceptionHandler.Exceptions;

namespace PersonalProjectApi.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger; 
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error"),
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                problemDetails.Detail = "Internal server error";
                _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
            }
            else if (statusCode == StatusCodes.Status404NotFound)
            {
                problemDetails.Detail = "Resource was not found";
                _logger.LogInformation("Resource was not found.");
            }
            else
            {
                problemDetails.Detail = exception.Message;
                _logger.LogWarning("Request rejected with status {StatusCode}: {Message}", statusCode, exception.Message);
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
