using HireConnect.Auth.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace HireConnect.Auth.Middleware
{
    public class AuthExceptionHandler
    {
        private readonly RequestDelegate _next;

        public AuthExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
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
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred. Please try again later.";

            // Specific Database and Logic Mapping
            switch (exception)
            {
                case AuthException authEx:
                    statusCode = authEx.StatusCode;
                    message = authEx.Message;
                    break;

                case DbUpdateException dbEx:
                    // Check for unique constraint violations (e.g., duplicate email)
                    if (dbEx.InnerException?.Message.Contains("unique") == true ||
                        dbEx.InnerException?.Message.Contains("duplicate") == true)
                    {
                        statusCode = (int)HttpStatusCode.Conflict;
                        message = "This email address is already in use.";
                    }
                    else
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        message = "A database error occurred while saving your data.";
                    }
                    break;

                case TimeoutException:
                case OperationCanceledException:
                    statusCode = (int)HttpStatusCode.ServiceUnavailable;
                    message = "The request timed out. Please check your connection.";
                    break;

                default:
                    // Keep the generic 500 status and generic message for unknown errors
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                message = message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}