using Microsoft.AspNetCore.Mvc;
using Plantt.API.Configurations;
using Plantt.API.Middleware;
using Plantt.Domain.Config;
using Serilog;

namespace Plantt.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            var services = builder.Services;

            services.AddControllers(options =>
                options.Filters.Add(new ProducesAttribute("application/json"))
            );

            services.Configure<PasswordSettings>(config.GetSection("PasswordSettings"));
            services.Configure<RefreshTokenSettings>(config.GetSection("RefreshTokenSettings"));
            services.Configure<HubSettings>(config.GetSection("HubSettings"));

            // These services can be found in DependencyInjection.cs
            services
                .AddJsonWebTokenAuthentication(config)
                .AddPlanttAuthorization()
                .AddEntityServices()
                .AddMiddleware()
                .AddInfrastructure(config)
                .AddValidator()
                .AddSwagger()
                .AddVersioning()
                .AddMapper();

            // Logging setup
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseSerilogRequestLogging();

            // This is commented out as there were some problems getting certificates to work correctly -
            // on the android app and microcontroller, worked fine on postman though.

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<AccountFromTokenMiddleware>();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}