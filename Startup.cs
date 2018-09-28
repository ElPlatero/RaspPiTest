using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaspPiTest.FritzBox;
using RaspPiTest.Hue;
using RaspPiTest.Weather;

namespace RaspPiTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                .AddJsonFile("fritzbox-settings.json", true)
                .Build();
            services.Configure<WeatherOptions>(configuration.GetSection("weather"));
            services.Configure<FritzBoxConnection>(configuration.GetSection("connection"));
            services.Configure<HueConfiguration>(configuration.GetSection("hue"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("allowDevAngular");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("webapi", "{controller}/{action=Index}");
                routes.MapRoute("ng", "{*url}", new { controller = "Home", action = "Index" });
            });
        }
    }
}
