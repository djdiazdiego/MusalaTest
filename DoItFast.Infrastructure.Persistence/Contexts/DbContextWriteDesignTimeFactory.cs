using DoItFast.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace DoItFast.Infrastructure.Persistence.Contexts
{
    public class DbContextWriteDesignTimeFactory : IDesignTimeDbContextFactory<DbContextWrite>
    {
        public DbContextWrite CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            Trace.WriteLine($"BasePath: {basePath}");
            var configuration = new ConfigurationBuilder()
               .SetBasePath(basePath)
               .AddJsonFile("appsettings.json")
               .AddJsonFile($"appsettings.{environment}.json")
               .Build();

            var sqlServerSettings = configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>();
            var connectionString = configuration.GetConnectionString("SQLServerCS");

            var builder = new DbContextOptionsBuilder<DbContextWrite>();
            builder.UseSqlServer(connectionString, opts =>
            {
                opts.MigrationsAssembly(sqlServerSettings.MigrationsAssemblyName.Persistence);
                opts.EnableRetryOnFailure(
                    maxRetryCount: sqlServerSettings.MaxRetryCount,
                    maxRetryDelay: sqlServerSettings.MaxRetryDelay,
                    errorNumbersToAdd: sqlServerSettings.ErrorNumbersToAdd);
            });

            return new DbContextWrite(builder.Options);
        }
    }
}