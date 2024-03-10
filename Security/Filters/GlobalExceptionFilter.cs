using BasketballManagerAPI.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Security.Filters {
    public class GlobalExceptionFilter : IExceptionFilter {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) {
            _logger = logger;
        }

        public void OnException(ExceptionContext context) {
            var problemDetails = GetProblemDetails(context.Exception);

            if (problemDetails.Status >= StatusCodes.Status500InternalServerError) {
                _logger.LogError(context.Exception, "Server error occurred.");
            } else {
                _logger.LogWarning(context.Exception, "Client error occurred.");
            }

            context.HttpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            context.HttpContext.Response.ContentType = "application/problem+json";
            context.Result = new ObjectResult(problemDetails) {
                StatusCode = problemDetails.Status
            };
            context.ExceptionHandled = true;
        }

        private ProblemDetails GetProblemDetails(Exception exception) {
            return exception switch {
                NotFoundException notFoundEx => new ProblemDetails {
                    Title = "Resource not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = notFoundEx.Message
                },
                AuthException authException => new ProblemDetails {
                    Title = "Auth validation error",
                    Status = authException.StatusCode,
                    Detail = authException.Message
                },
                BadRequestException badReqEx => new ProblemDetails {
                    Title = "Bad request",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = badReqEx.Message
                },
                _ => new ProblemDetails {
                    Title = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message
                }
            };
        }
    }
}
