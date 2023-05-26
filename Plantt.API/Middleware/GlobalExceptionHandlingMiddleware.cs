using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Plantt.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Unhandled exception occured: {target} {stacktrace} {innerException}", error.TargetSite, error.StackTrace, error.InnerException);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new ProblemDetails()
                {
                    Title = "An error occurred",
                    Detail = error.Message,
                    Status = StatusCodes.Status500InternalServerError
                };

                string problemJson = JsonSerializer.Serialize(problem);

                context.Response.ContentType = ContentType.ApplicationJson.ToString();

                await context.Response.WriteAsync(problemJson);

                throw;
            }
        }
    }
}
