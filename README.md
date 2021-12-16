# Open Telemetry .Net Demo
This demo is a simple .net Demo showcasing how to use Open Telemetry .Net SDK in your projects.

## DEMO 001
To start the demo follow the following steps:

### Create a simple WebApi Project
Simply create a new directory for your app and then generate a new sample .Net WebApi App
```
mkdir app
cd app
dotnet new webapi
```

### Build and Run the Project and Test the app's swagger file
Simply open Visual Studio Code on this file and run it. Then on the browser window open `https://localhost:5000/swagger`
NOTE: use the correct Port that your project generated, it probably is not `5000`

### Add references to the OpenTelemetry Packages to the .csproj
Simply edit your `.csproj` file and add the following code to the `<ItemGoup>` section.

```XML
    <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.0.1" />
    <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.0.1" />
    <PackageReference Include="OpenTelemetry.Exporter.Console" Version="1.0.1" />
```
Visual Studio code should see the update made to the project and missing packages and ask to reload, say yes and the packages should automaticaly load

### update the app startup to set the OpenTelemetry Instrumentation, Extentions and Exporter
To do this open the `program.cs` file and simply add the following code
```C#
builder.Services.AddOpenTelemetryTracing((builder) => builder
   .SetResourceBuilder(
      ResourceBuilder.CreateDefault().AddService("example-app"))
         .AddAspNetCoreInstrumentation()
         .AddConsoleExporter());
```
This code performs 3 main things
- Enables tracing for basic AspNet activity, including all incoming http requests.
- Sets the service name to “example-app,” which is how this service will be identified in the traces.
- Adds a console exporter, which will log traces to stdout.

### Call the WeatherForcast Service and view the Trace Log
You can now run the app again and call the WeatherForecast service using the swagger page and in Visual Studio Code, on the Debug Console you will see that that teh trace logs are showing in the OpenTelemetry format as shown bellow:
```
Activity.Id:          00-c56481a95ca04f497f1af2d5b140bcea-6d0ae74f0b7c42d5-01
Activity.DisplayName: WeatherForecast
Activity.Kind:        Server
Activity.StartTime:   2021-12-16T00:00:52.8492943Z
Activity.Duration:    00:00:00.0006861
Activity.TagObjects:
    http.host: localhost:7209
    http.method: GET
    http.target: /WeatherForecast
    http.url: https://localhost:7209/WeatherForecast
    http.user_agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 Edg/96.0.1054.57
    http.route: WeatherForecast
    http.status_code: 200
    otel.status_code: UNSET
Resource associated with Activity:
    service.name: example-app
    service.instance.id: 7b53b833-97ff-48bd-86ac-de3a2188e2b0

```

you can use other exporters from other nuget packages to export your logs to any OpenTelemetry compatible endpoints by simply changing the `program.cs` file as we did and replace/add your new exporter to the `AddConsoleExporter()`

## Simple logger Console App
Now lets try a simple logger console app to see how this is done using the ILogger implementation of OpenTelemetry .Net SDK

Simply create a new console application using the template and add the regular logging to it and finally configure the OpenTelemetry .Net SDK on it

### Create a new Console Application and Run it

Create a new console application and run it:
```
dotnet new console --output getting-started
cd getting-started
dotnet run
```
You should see the following output:
```
Hello World!
```
### Add the Logging and OpenTelemetry .Net SDK
Install the latest `Microsoft.Extensions.Logging` and `OpenTelemetry.Exporter.Console` by adding the following to the `.csproj`

```XML
  <ItemGroup>
      <PackageReference Include="Microsoft.Extensions.Logging" Version="6.0.0" />
      <PackageReference Include="OpenTelemetry.Exporter.Console" Version="1.0.1" />
  </ItemGroup>
```

Now change the `Program.cs` file to add the follwing usings:
```C#
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
```

and now change the `Main` function to the following code:
```c#
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options => options
        .AddConsoleExporter());
});

var logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Hello from {name} {price}.", "tomato", 2.99);
```
## Run the sample again
Now simply run the console application again using the following console command
```
dotnet run
```
you should see the following output:
```
LogRecord.TraceId:            00000000000000000000000000000000
LogRecord.SpanId:             0000000000000000
LogRecord.Timestamp:          2021-12-16T00:38:05.1796692Z
LogRecord.EventId:            0
LogRecord.CategoryName:       Program
LogRecord.LogLevel:           Information
LogRecord.TraceFlags:         None
LogRecord.State:              Hello from tomato 2.99.
```

## DEMO 2


Now lets dive a little deeper in the code and add the full OpenTelemetry .Net SDK. this will allow us to focus on all aspects of Observability, from Metrics to Trace to Logging.



