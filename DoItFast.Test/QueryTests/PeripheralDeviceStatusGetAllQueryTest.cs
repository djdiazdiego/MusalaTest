using AutoMapper;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Application.Features.Queries.Gateway;
using DoItFast.Application.Features.Queries.PeripheralDeviceStatus;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using DoItFast.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test.QueryTests
{
    public class PeripheralDeviceStatusGetAllQueryTest
    {
        private SetupServices _setupServices;

        [SetUp]
        public async Task Setup()
        {
            _setupServices = new SetupServices();
        }

        [Test]
        public async Task GetAllPeripheralDeviceStatus()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var queryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
                var mapper = scope.ServiceProvider.GetService<IMapper>();

                var query = new PeripheralDeviceStatusGetAllQuery();
                var queryHandler = new PeripheralDeviceStatusGetAllQueryHandler(queryRepository, mapper);
                var queryResult = await queryHandler.Handle(query, default);

                Assert.NotNull(queryResult);
                Assert.IsTrue(queryResult.Succeeded);
                Assert.NotNull(queryResult.Data);
                Assert.AreEqual(2, queryResult.Data.Length);
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

    }
}