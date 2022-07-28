using AutoMapper;
using DoItFast.Application.ApiMessages;
using DoItFast.Application.Features.Command.Gateway;
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
    public class GatewayDeletePeripheralDeviceCommandValidatorTest
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
        public async Task PeripheralDeviceNotFound()
        {
            using var scope = _setupServices.CreateScope();
            var queryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayDeletePeripheralDeviceCommand
            {
                Id = System.Guid.NewGuid(),
                SerialNumber = "SN"
            };
            var validator = new GatewayDeletePeripheralDeviceCommandValidator(queryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotFound.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(PeripheralDevice), error.PropertyName);
        }

    }
}