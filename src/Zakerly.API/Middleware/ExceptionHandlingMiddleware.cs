using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Zakerly.Application.Common.Exceptions;

namespace Zakerly.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,

            ForbiddenException => StatusCodes.Status403Forbidden,

            UnauthorizedException => StatusCodes.Status401Unauthorized,

            InvalidCredentialsException => StatusCodes.Status401Unauthorized,

            ConflictException => StatusCodes.Status409Conflict,

            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem));
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status500InternalServerError => "Internal Server Error",
            StatusCodes.Status409Conflict => "Conflict",
            _ => "Error"
        };
    }
}