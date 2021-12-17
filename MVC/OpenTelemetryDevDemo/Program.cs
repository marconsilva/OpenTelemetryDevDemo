using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

using OpenTelemetry.Exporter;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;

namespace OpenTelemetryDevDemo
{
    public class Program
    {
        public static ActivitySource activitySource = new ActivitySource("MyApplicationActivitySource");
        public static Meter meter = new Meter("MyAppMetrics");
        public static Counter<int> requestCounter = meter.CreateCounter<int>("compute_requests");

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(LoggingBuilder =>
                    {
                        LoggingBuilder.AddOpenTelemetry((builder) =>
                        {
                            builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("example-app-logging"))
                                .AddConsoleExporter();
                            builder.IncludeFormattedMessage = true;
                            builder.IncludeScopes = true;
                            builder.ParseStateValues = true;
                        });
                    });
                });
    }
}
