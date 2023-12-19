using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using working_good.business.core.Exceptions;

namespace working_good.business.infrastructure.Exceptions;

internal sealed class CustomExceptionMiddleware(ILogger<CustomExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        var (statusCode, error) = exception switch
        {
            CustomException => (StatusCodes.Status400BadRequest, new {Exception = exception.GetType().Name, Message = exception.Message}),
            _ => (StatusCodes.Status500InternalServerError, new {Exception = "server error", Message = "There was an error"})
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
}