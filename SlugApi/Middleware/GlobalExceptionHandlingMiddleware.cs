using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace SlugApi.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
            => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
             }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                var (status, title, detail) = ex switch
                {
                    ArgumentException => (
                        HttpStatusCode.BadRequest,
                        "Bad Request",
                        ex.Message
                    ),
                    _ => (
                        HttpStatusCode.InternalServerError,
                        "Server Error",
                        "internal server error occurred"
                    )
                };

                context.Response.StatusCode = (int)status;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails
                {
                    Status = (int)status,
                    Title = title,
                    Detail = detail
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
