using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plantt.API.Constants;
using Plantt.API.Middleware;
using Plantt.Applcation.Automapper;
using Plantt.Applcation.Services;
using Plantt.Applcation.Services.EntityServices;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.Config;
using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services;
using Plantt.Domain.Interfaces.Services.EntityServices;
using Serilog;

namespace Plantt.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            var service = builder.Services;

            // Add services to the container.
            var jwtSection = config.GetSection("JWTSettings");
            var jwtSettings = jwtSection?.Get<JsonWebTokenSettings>();

            if (jwtSettings is null || jwtSection is null)
            {
                throw new ApplicationException("JWTSettings section not found in appsettings.");
            }

            // ConfigSettings
            service.Configure<JsonWebTokenSettings>(jwtSection);
            service.Configure<PasswordSettings>(config.GetSection("PasswordSettings"));
            service.Configure<RefreshTokenSettings>(config.GetSection("RefreshTokenSettings"));

            // Services
            service.AddTransient<IPasswordService, PasswordPBKDF2Service>();
            service.AddTransient<ITokenAuthenticationService, TokenAuthenticationService>();
            service.AddTransient<IAccountService, AccountService>();
            service.AddTransient<IPlantService, PlantService>();
            service.AddTransient<IHubService, HubService>();
            service.AddTransient<IHomeService, HomeService>();

            // Middleware
            service.AddTransient<GlobalExceptionHandlingMiddleware>();

            // Unit of work
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            // Fluent Validation
            service.AddFluentValidationAutoValidation();
            service.AddFluentValidationClientsideAdapters();
            service.AddValidatorsFromAssemblyContaining<Program>();

            // Automapper
            service.AddAutoMapper(typeof(AccountProfile));

            // Data context
            service.AddDbContext<PlanttDBContext>((provider, options) =>
            {
                options.UseSqlServer(config["ConnectionStrings:Plantt"]);
            });

            service.AddControllers();

            // Add Swagger
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();


            // Add versioning.
            service.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // Add Authentication
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(3),
                    ValidateLifetime = true,

                    ValidateIssuer = false,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.SecretKeyBytes),
                };
            });

            // Configure Authorization
            service.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizePolicies.Admin, policy => policy.RequireRole(AccountRoles.Admin.ToString()));

                options.AddPolicy(AuthorizePolicies.Premium, policy => policy.RequireRole(
                            AccountRoles.Admin.ToString(),
                            AccountRoles.Premium.ToString()
                        ));

                options.AddPolicy(AuthorizePolicies.Registered, policy => policy.RequireRole(
                            AccountRoles.Admin.ToString(),
                            AccountRoles.Premium.ToString(),
                            AccountRoles.Registred.ToString()
                        ));
            });

            //Logging setup
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));


            var app = builder.Build();

            //Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseSerilogRequestLogging();

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}