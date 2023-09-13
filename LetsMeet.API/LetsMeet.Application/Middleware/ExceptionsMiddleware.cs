using LetsMeet.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LetsMeet.Application.Middleware;

public class ExceptionsMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionsMiddleware> _logger;

    public ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ProjectException exception)
        {
            context.Response.StatusCode = (int)exception.ErrorCode;
            _logger.LogError(exception, "Error 400");
            await context.Response.WriteAsJsonAsync(new { Title = exception.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            _logger.LogError(ex, "Error 500");
            await context.Response.WriteAsJsonAsync(new { Title = "Błąd serwera" });
        }
    }
}