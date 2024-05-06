using Domain.utils;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.MiddleWares;

public class GlobalExceptionHandler : IExceptionHandler {
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) {
        _logger = logger;
    }


    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken) {
        ProblemDetails problemDetails;

        if (exception is ValidationException validationException) {
            problemDetails = new ProblemDetails() {
                Type = validationException.Type,
                Status = (int) validationException.ErrorCode,
                Detail = validationException.Message
            };
        
        }
        else {
            _logger.LogError("Exception occurred: {Exception}", exception.Message);
            problemDetails = new ProblemDetails() {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred! Please try again later."
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}