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
            AuthorizeCustomException => (StatusCodes.Status400BadRequest, new {ExceptionCode = "authorize_exception", Message = "There was a problem during authorize"}),
            CustomException customException => (StatusCodes.Status400BadRequest, new {ExceptionCode = customException.MessageCode, Message = customException.Message}),
            _ => (StatusCodes.Status500InternalServerError, new {ExceptionCode = "server error", Message = "There was an error"})
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
}