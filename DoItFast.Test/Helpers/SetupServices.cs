using AutoMapper;
using DoItFast.Application;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Persistence;
using DoItFast.Infrastructure.Persistence.Contexts;
using DoItFast.Infrastructure.Shared.Services;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;

namespace DoItFast.Test.Helpers
{
    public class SetupServices : IDisposable
    {
        public ServiceProvider Provider { get; }

        public SetupServices()
        {
            var root = new InMemoryDatabaseRoot();
            var services = new ServiceCollection();
            services.AddSingleton<ISqlGuidGenerator, SequentialGuidGeneratorService>();
            services.AddDbContext<DbContextWrite>(options => options.UseInMemoryDatabase("TestDb", root), ServiceLifetime.Singleton);
            services.AddDbContext<DbContextRead>(options => options.UseInMemoryDatabase("TestDb", root), ServiceLifetime.Singleton);
            services.AddRepositories();
            services.AddApplicationLayerServices(null);
            Provider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            Provider.Dispose();
        }
    }
}
