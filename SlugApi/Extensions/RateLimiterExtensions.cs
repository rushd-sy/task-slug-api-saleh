using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace SlugApi.Extensions
{
    public static class RateLimiterExtensions
    {
        public static IServiceCollection AddRateLimiterPolicy(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            var rateLimitSetting = configuration.GetSection("RateLimiting");
            var permitLimit = rateLimitSetting.GetValue<int>("PermitLimit");
            var windowSeconds = rateLimitSetting.GetValue<int>("WindowSeconds");
            var queueLimit = rateLimitSetting.GetValue<int>("QueueLimit");

            if (permitLimit <= 0)
                throw new InvalidOperationException("RateLimiting:PermitLimit must be greater than zero.");
            if (windowSeconds <= 0)
                throw new InvalidOperationException("RateLimiting:WindowSeconds must be greater than zero.");
            if (queueLimit < 0)
                throw new InvalidOperationException("RateLimiting:QueueLimit must be zero or greater.");

            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                rateLimiterOptions.OnRejected = async (context, cancellationToken) =>
                {
                    var retryAfterSeconds = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                        ? (int?)((int)retryAfter.TotalSeconds)
                        : null;

                    if (retryAfterSeconds.HasValue)
                        context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds.Value.ToString();

                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status429TooManyRequests,
                        Title = "Too Many Requests",
                        Detail = "You have exceeded the allowed request limit. Please try again later.",
                        Instance = context.HttpContext.Request.Path
                    };

                    if (retryAfterSeconds.HasValue)
                        problemDetails.Extensions["retryAfter"] = retryAfterSeconds.Value;

                    await Results.Problem(problemDetails).ExecuteAsync(context.HttpContext);
                };

                rateLimiterOptions.AddPolicy("fixed-Ip", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = TimeSpan.FromSeconds(windowSeconds),
                            QueueLimit = queueLimit
                        }));
            });

            return services;
        }
    }

}
