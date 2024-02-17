using BasketballManagerAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Filters {
    public class GlobalExceptionFilter:IExceptionFilter {
        public void OnException(ExceptionContext context)
        {
            var apiResponse = context.Exception switch {
                NotFoundException ex => new NotFoundObjectResult(new { message = ex.Message }) {
                    StatusCode = StatusCodes.Status404NotFound
                },
                DomainException ex => new ObjectResult(new { message = ex.Message }) {
                    StatusCode = StatusCodes.Status409Conflict
                },
                BadRequestException ex => new BadRequestObjectResult(new { message = ex.Message }),
                _ => new ObjectResult(new { message = context.Exception.Message}) { StatusCode = StatusCodes.Status500InternalServerError }
            };
            context.Result = apiResponse;
            context.ExceptionHandled = true;
        }
    }
}
