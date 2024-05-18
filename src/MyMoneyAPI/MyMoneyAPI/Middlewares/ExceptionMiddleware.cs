using System.Text.Json;
using MyMoneyAPI.Common.Exceptions;

namespace MyMoneyAPI.Middlewares;


public class ExceptionMiddleware(RequestDelegate next,
    IWebHostEnvironment env)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (InvalidUserInputException ex)
        {
            await HandleInvalidUserInputExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            if (env.IsDevelopment())
            {
                throw;
            }
            else
            {
                await HandleExceptionAsync(context, ex);
            }
        }
    }

    private static Task HandleInvalidUserInputExceptionAsync(HttpContext context, InvalidUserInputException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        
        var result = JsonSerializer.Serialize(new { error = ex.Message });
        return context.Response.WriteAsync(result);
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
        return context.Response.WriteAsync(result);
    }
}
