using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using OpenTelemetry.Exporter;

namespace OpenTelemetryDevDemo
{
    public class Program
    {
        public static ActivitySource activitySource = new ActivitySource("MyApplicationActivitySource");
        public static Meter meter = new Meter("MyAppMetrics");
        public static Counter<int> requestCounter = meter.CreateCounter<int>("compute_requests");
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddOpenTelemetryTracing((builder) => builder
                                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("example-app-Tracing"))
                                .AddAspNetCoreInstrumentation()
                                .AddSource("MyApplicationActivitySource")
                                .AddConsoleExporter());

            builder.Services.AddOpenTelemetryMetrics((builder) => builder
                                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("example-app-Metrics"))
                                .AddAspNetCoreInstrumentation()
                                .AddMeter("MyAppMetrics"));

            builder.Logging.AddOpenTelemetry((builder) => {
                                builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("example-app-Logging"))
                                    .AddConsoleExporter();
                                builder.IncludeFormattedMessage = true;
                                builder.IncludeScopes = true;
                                builder.ParseStateValues = true;
            });
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}