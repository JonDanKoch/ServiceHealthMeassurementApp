using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using ServiceHealthMeassurementApp.Services;

var builder = WebApplication.CreateBuilder(args);

// API and Swagger boilerplate code 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    // Set up XML comments path for Swagger
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "ServiceHealthMeassurementApp.xml");
    options.IncludeXmlComments(xmlFile);
});

// Hosted Service to periodically discover new Kubernetes services
builder.Services.AddHostedService<ServiceDiscoveryService>();

// OpenTelemetry config to collect http metrics and log them to the console
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation() 
        .AddConsoleExporter()  // Export metrics to console
    )
    .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddMeter("ServiceHealthAppMetrics")
    )
    .WithMetrics(builder => builder
    .AddPrometheusExporter());


var app = builder.Build();

app.MapControllers();

app.UseOpenTelemetryPrometheusScrapingEndpoint();  // Exposes /metrics endpoint for Prometheus scraping

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
