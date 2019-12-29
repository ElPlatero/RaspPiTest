using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace RaspPiTest
{
    public class Program
    {
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
                .UseSerilog((hostingContext, logging) => {
                        logging.MinimumLevel.Debug();
                        logging.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information).WriteTo.Console();
                        logging
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .WriteTo.File("logs\\sssh..log", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}");
                })
                .UseConfiguration(new ConfigurationBuilder().AddJsonFile("hosting.json", true, false).Build())
                .UseStartup<Startup>();
        }
    }
}
