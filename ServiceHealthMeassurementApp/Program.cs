using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OpenTelemetry config to collect http metrics and log them to the console
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder => builder
        .AddPrometheusExporter())
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddAspNetCoreInstrumentation()  // Http metric collection
        .AddConsoleExporter()  // Export metrics to console
    )
    .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddMeter("ServiceHealthAppMetrics")
    );


var app = builder.Build();

app.MapControllers();

app.UseOpenTelemetryPrometheusScrapingEndpoint();  // Exposes /metrics endpoint for Prometheus scraping

app.Run();
