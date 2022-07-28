using AutoMapper;
using DoItFast.Application.ApiMessages;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Application.Features.Queries.Gateway;
using DoItFast.Application.Features.Queries.PeripheralDeviceStatus;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Extensions;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using DoItFast.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test.ValidationTests
{
    public class GatewayUpdatePeripheralDeviceCommandValidatorTest
    {
        private SetupServices _setupServices;
        private Guid _id;

        [SetUp]
        public async Task Setup()
        {
            _setupServices = new SetupServices();

            using var scope = _setupServices.CreateScope();
            var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
            var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

            _id = sqlGuidGenerator.NewGuid();

            var gateway = new Gateway("SN", "RN", "127.0.0.1");
            peripheralDeviceRepository.Add(gateway.AddPeripheralDevice(_id, "V1", PeripheralDeviceStatusValues.Online));
            peripheralDeviceRepository.Add(gateway.AddPeripheralDevice(sqlGuidGenerator.NewGuid(), "V2", PeripheralDeviceStatusValues.Offline));
            gatewayRepository.Add(gateway);
            await unitOfWork.SaveChangesAsync(default);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _setupServices.DisposeAsync();
        }

        [Test]
        public async Task GatewayPeripheralDevicesNotFound()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();
            var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdatePeripheralDeviceCommand
            {
                Id = sqlGuidGenerator.NewGuid(),
                PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                SerialNumber = "SN",
                Vendor = "V"
            };
            var validator = new GatewayUpdatePeripheralDeviceCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotFound.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(PeripheralDevice), error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesInvalidPeripheralDeviceStatusId()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayUpdatePeripheralDeviceCommand
            {
                Id = _id,
                PeripheralDeviceStatusId = default,
                SerialNumber = "SN",
                Vendor = "V"
            };
            var validator = new GatewayUpdatePeripheralDeviceCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GatewayMessages.PeripheralDeviceStatusNonexistent.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(PeripheralDevice.PeripheralDeviceStatusId), error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesVendorExceedsMaximumLength64()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdatePeripheralDeviceCommand
            {
                Id = _id,
                PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                SerialNumber = "SN",
                Vendor = "VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV"
            };
            var validator = new GatewayUpdatePeripheralDeviceCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual($"{GeneralMessages.MaximumLength.GetDescription()} 64", error.ErrorMessage);
            Assert.AreEqual(nameof(PeripheralDevice.Vendor), error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesVendorNull()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdatePeripheralDeviceCommand
            {
                Id = _id,
                PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                SerialNumber = "SN"
            };
            var validator = new GatewayUpdatePeripheralDeviceCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotNull.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(PeripheralDevice.Vendor), error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesVendorEmpty()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdatePeripheralDeviceCommand
            {
                Id = _id,
                PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                SerialNumber = "SN",
                Vendor = ""
            };
            var validator = new GatewayUpdatePeripheralDeviceCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotEmpty.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(PeripheralDevice.Vendor), error.PropertyName);
        }
    }
}