using DoItFast.Application.Behaviours;
using DoItFast.Application.Extensions;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Settings;
using DoItFast.Infrastructure.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace DoItFast.Application
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddApplicationLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddAuthenticationService(configuration);

            //services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(DbContextWrite).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<DbContextWrite>());

            services.AddMapperExtension(new Assembly[] {
                Assembly.GetExecutingAssembly()
            });

            services.AddServices();
        }

        /// <summary>
        /// Add authentication service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection(nameof(JWTSettings));
            var jwtSettings = configurationSection.Get<JWTSettings>();
            services.Configure<JWTSettings>(configurationSection);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;

                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };

                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    },
                };
            });
        }

        /// <summary>
        /// Add services.
        /// </summary>
        /// <param name="services"></param>
        private static void AddServices(this IServiceCollection services)
        {
            //services.AddScoped<IMailService, MailService>();
            //services.AddScoped<IAccountService, AccountService>();
        }

    }
}
