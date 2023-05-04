using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plantt.Applcation.Automapper;
using Plantt.Applcation.Services;
using Plantt.Applcation.Services.ControllerServices;
using Plantt.DataAccess.EntityFramework;
using Plantt.Domain.Config;
using Plantt.Domain.Interfaces.Services;

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
            service.AddTransient<IAccountControllerService, AccountControllerService>();

            // Fluent Validation
            service.AddFluentValidationAutoValidation();
            service.AddFluentValidationClientsideAdapters();
            service.AddValidatorsFromAssemblyContaining<Program>();

            // Automapper
            service.AddAutoMapper(typeof(AccountProfile));

            // Data context
            service.AddDbContext<PlanttDbContext>((provider, options) =>
            {
                options.UseSqlServer(config["ConnectionStrings:Plantt"]);
            });

            service.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();


            // Add versioning.
            service.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

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
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,

                    ValidateIssuer = false,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.SecretKeyBytes),
                };
            });

            var app = builder.Build();

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}