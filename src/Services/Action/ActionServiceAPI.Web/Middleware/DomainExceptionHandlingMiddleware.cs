using ActionServiceAPI.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ActionServiceAPI.Web.Middleware
{
    public class DomainExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<DomainExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ActionDomainException ex)
            {
                ProblemDetails details = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "Domain Failure",
                    Title = "Domain Error"
                };

                if (ex.InnerException is ValidationException)
                {
                    logger.LogTrace("Validation failed!");
                    details.Detail = "One or more validation errors has occurred";
                    var validationException = ex.InnerException as ValidationException;
                    details.Extensions.Add("ValidationErrors", validationException!.Errors);
                }
                else
                {
                    logger.LogError(ex, "Domain Exception thrown!");
                    details.Detail = ex.Message;
                }

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(details);
            }
        }
    }
}
