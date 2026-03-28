using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Exceptions;

namespace TaskFlow.Api.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            var (statusCode, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request"),
                ValidationAppException => (StatusCodes.Status400BadRequest, "Validation Error"),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = $"https://httpstatuses.com/{statusCode}",
                Instance = $"{context.Request.Method} {context.Request.Path}"
            };

            problemDetails.Detail = _env.IsDevelopment() ? exception.Message : "An error occurred while processing your request.";

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            if (exception is ValidationAppException validationEx)
            {
                problemDetails.Extensions["errors"] = validationEx.Errors;
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
