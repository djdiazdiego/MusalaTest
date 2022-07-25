using DoItFast.Domain;
using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Settings;
using DoItFast.Infrastructure.Persistence.Contexts;
using DoItFast.Infrastructure.Persistence.Repositories;
using DoItFast.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DoItFast.Infrastructure.Persistence
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
        public static void AddPersistenceInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var sqlServerSettings = configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>();
            var connectionString = configuration.GetConnectionString("SQLServerCS");

            services.AddDbContext<DbContextWrite>(options => options.ConfigureOptions(sqlServerSettings, connectionString), ServiceLifetime.Scoped);
            services.AddDbContext<DbContextRead>(options => options.ConfigureOptions(sqlServerSettings, connectionString), ServiceLifetime.Scoped);
            services.AddRepositories();
        }

        /// <summary>
        /// Add repositories services.
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.Load("DoItFast.Domain");
            var types = typeof(IEntity).GetConcreteTypes(assembly);

            foreach (var type in types)
            {
                if (type.GetInterface(nameof(INotRepository)) == null)
                {
                    CreateRepositories(services, type, typeof(IRepository<>), typeof(Repository<>), typeof(DbContextWrite));
                }
                CreateRepositories(services, type, typeof(IQueryRepository<>), typeof(QueryRepository<>), typeof(DbContextRead));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="entityType"></param>
        /// <param name="interfacetype"></param>
        /// <param name="concreteType"></param>
        /// <param name="dbContextType"></param>
        private static void CreateRepositories(IServiceCollection services, Type entityType, Type interfacetype, Type concreteType, Type dbContextType)
        {
            interfacetype = interfacetype.MakeGenericType(entityType);
            concreteType = concreteType.MakeGenericType(entityType);

            services.AddScoped(interfacetype, provider => Activator.CreateInstance(concreteType, provider.GetService(dbContextType)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="sqlServerSettings"></param>
        /// <param name="connectionString"></param>
        private static void ConfigureOptions(
            this DbContextOptionsBuilder options,
            SqlServerSettings sqlServerSettings,
            string connectionString)
        {
            options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(sqlServerSettings.MigrationsAssemblyName.Persistence);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: sqlServerSettings.MaxRetryCount,
                    maxRetryDelay: sqlServerSettings.MaxRetryDelay,
                    errorNumbersToAdd: sqlServerSettings.ErrorNumbersToAdd);
            });
        }
    }

}
