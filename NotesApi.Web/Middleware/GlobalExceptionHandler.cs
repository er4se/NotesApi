using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Domain.Common.Exceptions;

namespace NotesApi.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is FluentValidation.ValidationException validationException)
            {
                _logger.LogWarning(exception, "Validation failed");

                var errors = validationException.Errors.Select(e => new {
                    e.PropertyName,
                    e.ErrorMessage
                });

                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = "One or more validation errors occurred.",
                    Extensions = { ["errors"] = errors }
                };

                httpContext.Response.StatusCode = 400;
                httpContext.Response.ContentType = "application/problem+json";
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                
                return true;
            }
            else
            {
                _logger.LogError(exception, "Unhandled exception occurred");

                var (status, title) = MapException(exception);

                var problemDetails = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path,
                    Type = exception.GetType().Name
                };

                httpContext.Response.StatusCode = status;
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }
        }

        public static (int status, string title) MapException(Exception ex)
        {
            return ex switch
            {
                FluentValidation.ValidationException => (400, "Validation Error"),
                NotFoundException => (404, "Not Found"),
                ConflictException => (409, "Conflict"),
                ForbiddenException => (403, "Forbidden"),
                _ => (500, "Internal Server Error")
            };
        }
    }
}
