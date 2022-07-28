using AutoMapper;
using DoItFast.Application.ApiMessages;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Extensions;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using DoItFast.Test.Setup;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test.ValidationTests
{
    public class GatewayCreateCommandValidatorTest
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
        public async Task GatewaySerialNumberNull()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotNull.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.SerialNumber), error.PropertyName);
        }

        [Test]
        public async Task GatewaySerialNumberEmpty()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotEmpty.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.SerialNumber), error.PropertyName);
        }

        [Test]
        public async Task GatewaySerialNumberExceedsMaximumLength32()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual($"{GeneralMessages.MaximumLength.GetDescription()} 32", error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.SerialNumber), error.PropertyName);
        }

        [Test]
        public async Task GatewaySerialNumberIncorrectComposition()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "sn",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GatewayMessages.SerialNumberIncorrectComposition.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.SerialNumber), error.PropertyName);
        }

        [Test]
        public async Task GatewaySerialNumberAlreadyExists()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
            var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

            var commad = new GatewayCreateCommand
            {
                SerialNumber = "SN",
                IpAddress = "127.0.0.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
                {
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        Vendor= "V1",
                        PeripheralDeviceStatusId= PeripheralDeviceStatusValues.Online
                    },
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        Vendor= "V2",
                        PeripheralDeviceStatusId= PeripheralDeviceStatusValues.Offline
                    }
                }
            };
            var commandHandler = new GatewayCreateCommandHandler(gatewayRepository, peripheralDeviceRepository, mapper, unitOfWork, sqlGuidGenerator);
            var result = await commandHandler.Handle(commad, default);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var validatoResult = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(validatoResult);
            Assert.IsFalse(validatoResult.IsValid);
            Assert.AreEqual(1, validatoResult.Errors.Count);

            var error = validatoResult.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.AlreadyExists.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.SerialNumber), error.PropertyName);
        }

        [Test]
        public async Task GatewayReadableNameNull()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotNull.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.ReadableName), error.PropertyName);
        }

        [Test]
        public async Task GatewayReadableNameEmpty()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotEmpty.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.ReadableName), error.PropertyName);
        }

        [Test]
        public async Task GatewayReadableNameExceedsMaximumLength64()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual($"{GeneralMessages.MaximumLength.GetDescription()} 64", error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.ReadableName), error.PropertyName);
        }

        [Test]
        public async Task GatewayIpAddressNull()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotNull.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.IpAddress), error.PropertyName);
        }

        [Test]
        public async Task GatewayIpAddressEmpty()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotEmpty.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.IpAddress), error.PropertyName);
        }

        [Test]
        public async Task GatewayIpAddressIncorrectComposition()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.InvalidIpAddress4.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.IpAddress), error.PropertyName);
        }

        [Test]
        public async Task GatewayExceedsMaximumPeripheralDevicesNumber10()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            for (int i = 0; i < 6; i++)
            {
                peripheralDevices.Add(new GatewayCreateCommand.PeripheralDeviceModel
                {
                    PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Offline,
                    Vendor = "V1"
                });
                peripheralDevices.Add(new GatewayCreateCommand.PeripheralDeviceModel
                {
                    PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                    Vendor = "V2"
                });
            }

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = peripheralDevices
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GatewayMessages.ExceededPeripheralDeviceNumber.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.PeripheralDevices), error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesInvalidStatus()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
                {
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = default,
                        Vendor = "V"
                    }
                }
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GatewayMessages.PeripheralDeviceStatusNonexistent.GetDescription(), error.ErrorMessage);
            Assert.AreEqual("PeripheralDevices[0].PeripheralDeviceStatusId", error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesVendorNull()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
                {
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online
                    }
                }
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotNull.GetDescription(), error.ErrorMessage);
            Assert.AreEqual("PeripheralDevices[0].Vendor", error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesVendorEmpty()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
                {
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                        Vendor = ""
                    }
                }
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotEmpty.GetDescription(), error.ErrorMessage);
            Assert.AreEqual("PeripheralDevices[0].Vendor", error.PropertyName);
        }

        [Test]
        public async Task GatewayPeripheralDevicesVendorExceedsMaximumLength64()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var peripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>();

            var query = new GatewayCreateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>()
                {
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                        Vendor = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                    }
                }
            };
            var validator = new GatewayCreateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual($"{GeneralMessages.MaximumLength.GetDescription()} 64", error.ErrorMessage);
            Assert.AreEqual("PeripheralDevices[0].Vendor", error.PropertyName);
        }
    }
}