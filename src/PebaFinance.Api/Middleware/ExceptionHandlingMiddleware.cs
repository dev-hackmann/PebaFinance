using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using PebaFinance.Application.Exceptions;

namespace PebaFinance.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly Dictionary<Type, HttpStatusCode> ExceptionStatusCodeMapping = new()
    {
        { typeof(DuplicateDescriptionException), HttpStatusCode.Conflict },
        { typeof(InvalidCategoryException), HttpStatusCode.BadRequest}
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = ExceptionStatusCodeMapping.TryGetValue(exception.GetType(), out var code)
            ? code
            : HttpStatusCode.InternalServerError;

        var response = new
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = statusCode == HttpStatusCode.InternalServerError
                ? "An unexpected error occurred."
                : exception.Message,
            Status = (int)statusCode,
            Errors = new Dictionary<string, string[]>(),
            TraceId = context.TraceIdentifier
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
