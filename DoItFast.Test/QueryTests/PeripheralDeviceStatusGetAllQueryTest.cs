using AutoMapper;
using DoItFast.Application.Features.Queries.PeripheralDeviceStatus;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Test.Setup;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
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

        [TearDown]
        public async Task TearDown()
        {
            await _setupServices.DisposeAsync();
        }

        [Test]
        public async Task GetAllPeripheralDeviceStatus()
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

    }
}