using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;

namespace Monitoring;

public class MonitoringService
{
    public static readonly string ServiceName = Assembly.GetExecutingAssembly().GetName().Name ?? "Unknown";
    public static TracerProvider TracerProvider;
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
    public static ILogger Log => Serilog.Log.Logger;

    static MonitoringService()
    {
        // Serilog
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Seq("http://localhost:5341")
            .Enrich.WithSpan()
            .CreateLogger();

        // OpenTelemetry

        TracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddZipkinExporter()
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder
                .CreateDefault()
                .AddService(ServiceName))
            .Build();
    }
}