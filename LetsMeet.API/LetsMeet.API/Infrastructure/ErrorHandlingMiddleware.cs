using LetsMeet.API.Exceptions;

namespace LetsMeet.API.Infrastructure;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
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
