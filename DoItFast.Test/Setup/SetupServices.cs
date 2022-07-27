using DoItFast.Application;
using DoItFast.Infrastructure.Persistence;
using DoItFast.Infrastructure.Persistence.Contexts;
using DoItFast.Infrastructure.Persistence.Seeds;
using DoItFast.Infrastructure.Shared.Services;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DoItFast.Test.Setup
{
    public class SetupServices : IAsyncDisposable
    {
        private const string ConnectionString = "Data Source=InMemorySample;Mode=Memory;Cache=Shared";
        private readonly SqliteConnection _sqliteConnection;
        private IServiceCollection _services;
        private readonly ServiceProvider _provider;

        public SetupServices()
        {
            _sqliteConnection = new SqliteConnection(ConnectionString);
            _sqliteConnection.Open();

            _services = new ServiceCollection();
            _services.AddSingleton<ISqlGuidGenerator, SequentialGuidGeneratorService>();
            _services.AddDbContext<DbContextWrite>(options => options.UseSqlite(_sqliteConnection), ServiceLifetime.Scoped);
            _services.AddDbContext<DbContextRead>(options => options.UseSqlite(_sqliteConnection), ServiceLifetime.Scoped);
            _services.AddRepositories();
            _services.AddApplicationLayerServices(null);
            _provider = _services.BuildServiceProvider();

            Task.Run(async () => await LoadSeedsAsync()).Wait();
        }

        public IServiceScope CreateScope() => _provider.CreateScope();

        private async Task LoadSeedsAsync()
        {
            using var scope = _provider.CreateScope();
            var dbContextWrite = scope.ServiceProvider.GetService<DbContextWrite>();
            await dbContextWrite.Database.EnsureCreatedAsync(default);

            var seed = new PeripheralDeviceStatusSeed();
            await seed.SeedAsync(_provider, default);
        }

        public async ValueTask DisposeAsync()
        {
            await _provider.DisposeAsync();
            _services.Clear();
            _services = null;
            await _sqliteConnection.CloseAsync();
            await _sqliteConnection.DisposeAsync();
        }
    }
}
