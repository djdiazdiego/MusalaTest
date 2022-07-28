using AutoMapper;
using DoItFast.Application.ApiMessages;
using DoItFast.Application.Features.Queries.PeripheralDeviceStatus;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Extensions;
using DoItFast.Test.Setup;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test.ValidationTests
{
    public class PeripheralDeviceStatusGetQueryValidatorTest
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
        public async Task PeripheralDeviceStatusNotFound()
        {
            using var scope = _setupServices.CreateScope();
            var queryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new PeripheralDeviceStatusGetQuery(default);
            var validator = new PeripheralDeviceStatusGetQueryValidator(queryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotFound.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(PeripheralDeviceStatus.Id), error.PropertyName);
        }

    }
}