using System.Net.Http;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message;

        switch (exception)
        {
            case RestrictedException restrictedException:
                status = HttpStatusCode.Forbidden;
                message = restrictedException.Message;
                break;
            case InvalidArgumentException invalidArgumentException:
                status = HttpStatusCode.BadRequest;
                message = invalidArgumentException.Message;
                break;
            case NotFoundException notFoundException:
                status = HttpStatusCode.NotFound;
                message = notFoundException.Message;
                break;
            // Add other custom exceptions here
            default:
                status = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}
