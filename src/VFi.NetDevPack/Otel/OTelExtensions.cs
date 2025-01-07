using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace VFi.NetDevPack.Otel;

public static class OTelExtensions
{
    public static IWebHostBuilder AddOTelLogs(this IWebHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureLogging((context, builder) =>
        {
            builder.ClearProviders();
            builder.AddConsole();

            var logExporter = context.Configuration.GetValue<string>("UseLogExporter")?.ToLowerInvariant();
            switch (logExporter)
            {
                case "console":
                    builder.AddOpenTelemetry(options => { options.AddConsoleExporter(); });
                    break;
                case "otlp":
                    builder.AddOpenTelemetry(options =>
                    {
                        var serviceName = context.Configuration.GetValue<string>("Otlp:ServiceName");
                        var otlpEndpoint = context.Configuration.GetValue<string>("Otlp:Endpoint");
                        options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                            .AddService(serviceName));
                        options.AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Endpoint = new Uri(otlpEndpoint);
                        });
                    });
                    break;
                case "":
                case "none":
                    break;
            }

            builder.Services.Configure<OpenTelemetryLoggerOptions>(opt =>
            {
                opt.IncludeScopes = true;
                opt.ParseStateValues = true;
                opt.IncludeFormattedMessage = true;
            });
        });

        return hostBuilder;
    }

    public static IServiceCollection AddOTelTracing(this IServiceCollection services, IConfiguration config)
    {
        var tracingExporter = config.GetValue<string>("UseTracingExporter")?.ToLowerInvariant();

        switch (tracingExporter)
        {
            case "console":
                services
                    .AddOpenTelemetry()
                    .WithTracing(tracerProviderBuilder =>
                    {
                        tracerProviderBuilder
                            .AddSource("MassTransit") // https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/326
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddEntityFrameworkCoreInstrumentation()
                            .AddConsoleExporter();
                    });
                //services.Configure<AspNetCoreInstrumentationOptions>(config.GetSection("AspNetCoreInstrumentation"));
                //services.Configure<AspNetCoreInstrumentationOptions>(options => { options.Filter = _ => true; });
                break;
            case "otlp":
                services
                    .AddOpenTelemetry()
                    .WithTracing(tracerProviderBuilder =>
                    {
                        tracerProviderBuilder
                            .AddSource(
                                "MassTransit") // https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/326
                            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                                .AddService(config.GetValue<string>("Otlp:ServiceName")))
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddRedisInstrumentation()
                            .AddEntityFrameworkCoreInstrumentation(b => b.SetDbStatementForText = true)
                            .AddOtlpExporter(otlpOptions =>
                            {
                                otlpOptions.Endpoint = new Uri(config.GetValue<string>("Otlp:Endpoint"));
                            });
                    });
                break;
            case "":
            case "none":
                break;
        }

        return services;
    }

    public static IServiceCollection AddOTelMetrics(this IServiceCollection services, IConfiguration config)
    {
        var metricsExporter = config.GetValue<string>("UseMetricsExporter")?.ToLowerInvariant();

        var serviceName = config.GetValue<string>("Otlp:ServiceName") ?? "DEFAULT_SERVICE_NAME";

        void ConfigureResource(ResourceBuilder r) => r.AddService(serviceName,
            serviceVersion: "unknown", serviceInstanceId: Environment.MachineName);

        services.AddOpenTelemetry()
            .WithMetrics(bd =>
            {
                bd.ConfigureResource(ConfigureResource)
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddPrometheusExporter();

                switch (metricsExporter)
                {
                    case "console":
                        bd.AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                        {
                            exporterOptions.Targets = ConsoleExporterOutputTargets.Console;
                            metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 10000;
                        });
                        break;
                    case "otlp":
                        bd.AddOtlpExporter(otlpOptions =>
                        {
                            otlpOptions.Endpoint = new Uri(config.GetValue<string>("Otlp:Endpoint"));
                        });
                        break;
                    case "":
                    case "none":
                        break;
                }
            });
        return services;
    }
}
