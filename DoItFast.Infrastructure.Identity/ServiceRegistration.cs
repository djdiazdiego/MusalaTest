using DoItFast.Domain.Settings;
using DoItFast.Infrastructure.Identity.Contexts;
using DoItFast.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoItFast.Infrastructure.Identity
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
        public static void AddIdentityInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityCS");
            var sqlServerSettings = configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>();           

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(sqlServerSettings.MigrationsAssemblyName.Identity);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: sqlServerSettings.MaxRetryCount,
                        maxRetryDelay: sqlServerSettings.MaxRetryDelay,
                        errorNumbersToAdd: sqlServerSettings.ErrorNumbersToAdd);
                }));

            services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
           
        }
    }
}
