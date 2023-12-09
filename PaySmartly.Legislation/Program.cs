using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PaySmartly.Legislation.Services;

string ServiceName = "Legislation Service";

var builder = WebApplication.CreateBuilder(args);
AddOpenTelemetryLogging(builder);

builder.Services.AddGrpc();
AddOpenTelemetryService(builder);

var app = builder.Build();
app.MapGrpcService<LegislationService>();
app.Run();

void AddOpenTelemetryLogging(WebApplicationBuilder builder)
{
    builder.Logging.AddOpenTelemetry(options =>
    {
        ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault().AddService(ServiceName);

        options.SetResourceBuilder(resourceBuilder).AddConsoleExporter();
    });
}

void AddOpenTelemetryService(WebApplicationBuilder builder)
{
    OpenTelemetryBuilder openTelemetryBuilder = builder.Services.AddOpenTelemetry();

    openTelemetryBuilder = openTelemetryBuilder.ConfigureResource(resource => resource.AddService(ServiceName));

    openTelemetryBuilder = openTelemetryBuilder.WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation().AddConsoleExporter();
    });
    
    openTelemetryBuilder = openTelemetryBuilder.WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation().AddConsoleExporter();
    });
}
