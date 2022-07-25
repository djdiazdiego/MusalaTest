using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Persistence.Contexts;
using DoItFast.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Test.Helpers;
using Test.Setup;

namespace Test
{
    public class TestingGateway
    {
        private readonly SetupServices _setupServices;

        public TestingGateway()
        {
            _setupServices = new SetupServices();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task AddValues_ReadValues()
        {
            var scope = _setupServices.Services.BuildServiceProvider().CreateScope();
            var repository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
            repository.Add(new Gateway("SN", "RN", "1.1.1.1"));
            await _setupServices.UnitOfWork.SaveChangesAsync(_setupServices.CancellationToken);

            var any = await repository.FindAll().AnyAsync(_setupServices.CancellationToken);
            var count = await repository.FindAll().CountAsync(_setupServices.CancellationToken);

            Assert.IsTrue(any);
            Assert.IsTrue(count == 1);
        }
    }
}