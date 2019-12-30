using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace RaspPiTest
{
    public class Program
    {
        private const string ApplicationName = "SmartHomeHub";
        private const string LogTemplate = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .AddJsonFile("appsettings.json", true, true)
                        .AddEnvironmentVariables();
                })
                .UseSerilog((hostingContext, logging) =>
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    logging
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .WriteTo.Console(outputTemplate: LogTemplate)
                        .WriteTo.File(Path.Combine(path, $"{ApplicationName}..log"), rollingInterval: RollingInterval.Day, outputTemplate: LogTemplate);
                })
                .UseConfiguration(new ConfigurationBuilder().AddJsonFile("hosting.json", true, false).Build())
                .UseStartup<Startup>();
        }
    }
}
