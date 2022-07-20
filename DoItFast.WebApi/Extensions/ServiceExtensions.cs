using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DoItFast.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddWebApiServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerExtension();
            builder.AddCorsExtensions();
            builder.Services.AddControllerExtension();
            builder.Services.AddApiVersioningExtension();
            builder.Services.AddSpaExtensions();
        }

        /// <summary>
        /// Register swagger
        /// </summary>
        /// <param name="services"></param>
        private static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(t => t.FullName);

                options.UseInlineDefinitionsForEnums();
                options.DescribeAllParametersInCamelCase();
                options.UseAllOfToExtendReferenceSchemas();

                var versions = new[] { "v1" };

                foreach (var version in versions)
                {
                    options.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = "MusalaTest.WebApi",
                        Version = version,
                        Description = "Api Documentation",
                        Contact = new OpenApiContact
                        {
                            Name = "Dayron Jesús Díaz Diego",
                            Email = "dj.diazdiego@gmail.com"
                        }
                    });
                }

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                //});
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer",
                //            },
                //            Scheme = "Bearer",
                //            Name = "Bearer",
                //            In = ParameterLocation.Header,
                //        }, new List<string>()
                //    },
                //});

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        /// <summary>
        /// Register api versioning
        /// </summary>
        /// <param name="services"></param>
        private static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
        }

        /// <summary>
        /// Register mvc builder
        /// </summary>
        /// <param name="services"></param>
        private static void AddControllerExtension(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        }

        /// <summary>
        /// Register spa
        /// </summary>
        /// <param name="services"></param>
        private static void AddSpaExtensions(this IServiceCollection services)
        {
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        /// <summary>
        /// Register cors
        /// </summary>
        /// <param name="builder"></param>
        private static void AddCorsExtensions(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(
                        name: "AllowOrigin", builder =>
                        {
                            builder.WithOrigins("http://localhost:4200")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
                });
            }
        }

        ///// <summary>
        ///// Register authentication
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="configuration"></param>
        //public static void AddAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddAuthentication(x =>
        //    {
        //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    }).AddJwtBearer();
        //}

        ///// <summary>
        ///// Register http clients from assembly
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="configuration"></param>
        //public static void AddHttpClientsExtension(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var httpClientSettings = configuration.GetSection("AppSettings").Get<HttpClientSettings>();

        //    var addAssemblyHttpClient = typeof(ServiceExtensions)
        //          .GetMethod(nameof(AddHttpClient), BindingFlags.NonPublic | BindingFlags.Static);
        //    var types = typeof(IClientService).GetConcreteTypes();

        //    foreach (var type in types)
        //    {
        //        var typeInterface = type.GetInterfaces()
        //            .Where(p => p != typeof(IClientService))
        //            .First();

        //        FieldInfo info = type.GetField("_attempts", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //        var attempts = info != null ? info.GetValue(null) : httpClientSettings.Attempts;

        //        var methodInfo = addAssemblyHttpClient.MakeGenericMethod(typeInterface, type);
        //        methodInfo.Invoke(null, new object[] { services, attempts });
        //    }
        //}

        ///// <summary>
        ///// Add http clients
        ///// </summary>
        ///// <typeparam name="TClient"></typeparam>
        ///// <typeparam name="TImplementation"></typeparam>
        ///// <param name="services"></param>
        ///// <param name="attempts"></param>
        //private static void AddHttpClient<TClient, TImplementation>(IServiceCollection services, int attempts)
        //    where TClient : class
        //    where TImplementation : class, TClient =>
        //    services.AddHttpClient<TClient, TImplementation>().AddPolicyHandler(GetRetryPolicy(attempts));

        ///// <summary>
        ///// Retry policy
        ///// </summary>
        ///// <param name="attempts"></param>
        ///// <returns></returns>
        //private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int attempts)
        //{
        //    return HttpPolicyExtensions
        //    .HandleTransientHttpError()
        //    //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        //    .WaitAndRetryAsync(attempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        //}
    }
}
