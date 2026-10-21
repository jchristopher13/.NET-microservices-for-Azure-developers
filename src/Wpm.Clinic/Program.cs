using Wpm.Clinic.ExternalServices;
using Wpm.Clinic.DataAccess;
using Wpm.Clinic.Application;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Microsoft.Extensions.Http.Resilience;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ManagementService>();
builder.Services.AddScoped<ClinicApplicationService>();

builder.Services.AddDbContext<ClinicDbContext>(options =>
{
    options.UseInMemoryDatabase("WpmClinic");
});

builder.Services.AddHttpClient<ManagementService>(client =>
{
    var uri =builder.Configuration.GetValue<string>("Wpm:ManagementUri");
    client.BaseAddress = new Uri(uri);
}).AddResilienceHandler("management-pipline", builder =>
{
        builder.AddRetry(new Polly.Retry.RetryStrategyOptions<HttpResponseMessage>()
        {
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Exponential,
            Delay = TimeSpan.FromSeconds(10)
        });

    // builder.AddRetry(new PollyStrategyOptions<HttpResponseMessage>()
    // {
    //     BackoffType = DelayBackoffType.Exponential,
    //     MaxRetryAttempts = 3,
    //     Delay = TimeSpan.FromSeconds(10)
    // });
});

var app = builder.Build();

app.EnsureClinicDbIsCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
