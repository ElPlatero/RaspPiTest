﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaspPiTest.FritzBox;
using RaspPiTest.Hue;
using RaspPiTest.Middleware;
using RaspPiTest.Weather;

namespace RaspPiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<FritzBoxClient>();
            services.AddSingleton<WeatherRepository>();
            services.AddSingleton<HueRepository>();
            services.AddCors(o => o.AddPolicy("allowDevAngular", builder =>
            {
                builder.WithOrigins("http://localhost:4200");
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            }));

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.owm.json", false)
                .AddJsonFile("fritzbox-settings.json", false)
                .Build();
            services.Configure<OpenWeatherMapConfiguration>(configuration);
            services.Configure<WeatherOptions>(configuration.GetSection("weather"));
            services.Configure<FritzBoxConnection>(configuration.GetSection("connection"));
            services.Configure<HueConfiguration>(configuration.GetSection("hue"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorMiddleware>();
            app.UseCors("allowDevAngular");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute("webapi", "{controller}/{action=Index}");
                routes.MapControllerRoute("ng", "{*url}", new { controller = "Home", action = "Index" });
            });
        }
    }
}
