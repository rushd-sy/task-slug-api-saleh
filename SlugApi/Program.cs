using FluentValidation;
using SlugApi.DTOs;
using SlugApi.Filters;
using SlugApi.Interfaces;
using SlugApi.Middleware;
using SlugApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ValidationFilter<GenerateSlugRequest>));
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IGenerateSlugServices, GenerateSlugService>();
builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();
builder.Services.AddScoped(typeof(ValidationFilter<>));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
