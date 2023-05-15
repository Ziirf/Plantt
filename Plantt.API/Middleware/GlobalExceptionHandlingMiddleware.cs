using Azure.Core;
using Microsoft.AspNetCore.Mvc;
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
                _logger.LogError(error, error.Message);

                int statusCode = StatusCodes.Status500InternalServerError;

                context.Response.StatusCode = statusCode;

                ProblemDetails problem = new ProblemDetails()
                {
                    Title = "An error occurred",
                    Detail = error.Message,
                    Status = statusCode
                };
                context.Response.ContentType = ContentType.ApplicationJson.ToString();

                string problemJson = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(problemJson);
            }
        }
    }
}
