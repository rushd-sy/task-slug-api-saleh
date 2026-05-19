using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using SlugApi.Filters;
using SlugApi.Interfaces;
using SlugApi.Middleware;
using SlugApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IGenerateSlugServices, GenerateSlugService>();
builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var rateLimitSetting = builder.Configuration.GetSection("RateLimiting");
builder.Services.AddRateLimiter(RateLimiterOptions =>
{
    RateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    RateLimiterOptions.AddPolicy("fixed-Ip", httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = rateLimitSetting.GetValue<int>("PermitLimit"),
            Window = TimeSpan.FromSeconds(rateLimitSetting.GetValue<int>("WindowSeconds")),
            QueueLimit = rateLimitSetting.GetValue<int>("QueueLimit")
        }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("fixed-Ip");

app.Run();
