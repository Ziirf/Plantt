using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
using System.Net;

namespace Plantt.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPlanttServices(this IServiceCollection services)
        {
            services.AddTransient<IPasswordService, PasswordPBKDF2Service>();
            services.AddTransient<ITokenAuthenticationService, TokenAuthenticationService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAccountPlantService, AccountPlantService>();
            services.AddTransient<IPlantService, PlantService>();
            services.AddTransient<IHubService, HubService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<ISensorService, SensorService>();

            return services;
        }

        public static IServiceCollection AddPlanttMiddleware(this IServiceCollection services)
        {
            services.AddTransient<GlobalExceptionHandlingMiddleware>();
            services.AddTransient<TokenHandlingMiddleware>();

            return services;
        }

        public static IServiceCollection AddJsonWebTokenAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var jwtSection = config.GetSection("JWTSettings");
            var jwtSettings = jwtSection?.Get<JsonWebTokenSettings>();

            if (jwtSettings is null || jwtSection is null)
            {
                throw new ApplicationException("JWTSettings section not found in appsettings.");
            }

            // ConfigSettings
            services.Configure<JsonWebTokenSettings>(jwtSection);

            services.AddAuthentication(options =>
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

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.SecretKeyBytes),
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        var endpoint = context.HttpContext.GetEndpoint();

                        if (endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() is null)
                        {
                            return Task.CompletedTask;
                        }

                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Authentication-Failure-Reason", "Token Expired");
                        }
                        else
                        {
                            context.Response.Headers.Add("Authentication-Failure-Reason", "Invalid Token");
                        }

                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddPlanttAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizePolicyConstant.Admin, policy => policy.RequireRole(AccountRoles.Admin.ToString()));

                options.AddPolicy(AuthorizePolicyConstant.Premium, policy => policy.RequireRole(
                            AccountRoles.Admin.ToString(),
                            AccountRoles.Premium.ToString()
                        ));

                options.AddPolicy(AuthorizePolicyConstant.Registered, policy => policy.RequireRole(
                            AccountRoles.Admin.ToString(),
                            AccountRoles.Premium.ToString(),
                            AccountRoles.Registred.ToString()
                        ));

                options.AddPolicy(AuthorizePolicyConstant.Hub, policy => policy.RequireRole(
                            "Hub"
                        ));
            });

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<PlanttDBContext>((provider, options) =>
            {
                options.UseSqlServer(config["ConnectionStrings:Plantt"]);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddValidator(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Program>();

            return services;
        }

        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AccountProfile));

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Plantt API",
                    Description = "The api side of a 3 part product, which also includes an app and a microcontroller."
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
            });

            return services;
        }
    }
}
