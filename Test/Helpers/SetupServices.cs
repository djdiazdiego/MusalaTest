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
using System;
using System.Threading;

namespace Test.Helpers
{
    public class SetupServices : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly InMemoryDatabaseRoot _root;
        public IServiceCollection Services { get; private set; }
        public IMediator Mediator { get; private set; }
        public IMapper Mapper { get; private set; }
        public IUnitOfWork UnitOfWork { get; }
        public CancellationToken CancellationToken { get; private set; }


        public SetupServices()
        {
            _root = new InMemoryDatabaseRoot();
            Services = new ServiceCollection();
            _configuration = new ConfigurationBuilder().Build();
            Services.AddSingleton(_configuration);

            Services.AddSingleton<ISqlGuidGenerator, SequentialGuidGeneratorService>();

            Services.AddDbContext<DbContextWrite>(options => options.UseInMemoryDatabase("TestDb", _root), ServiceLifetime.Singleton);
            Services.AddDbContext<DbContextRead>(options => options.UseInMemoryDatabase("TestDb", _root), ServiceLifetime.Singleton);
            Services.AddRepositories();

            Services.AddApplicationLayerServices(_configuration);

            Mediator = Services.BuildServiceProvider().GetService<IMediator>();
            Mapper = Services.BuildServiceProvider().GetService<IMapper>();
            UnitOfWork = Services.BuildServiceProvider().GetService<IUnitOfWork>();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
            Mediator = null;
            Mapper = null;
            CancellationToken = CancellationToken.None;
            Services.Clear();
            Services = null;
        }
    }
}
