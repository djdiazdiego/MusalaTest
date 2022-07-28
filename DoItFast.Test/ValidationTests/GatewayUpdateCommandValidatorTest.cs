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
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test.ValidationTests
{
    public class GatewayUpdateCommandValidatorTest
    {
        private SetupServices _setupServices;

        [SetUp]
        public async Task Setup()
        {
            _setupServices = new SetupServices();

            using var scope = _setupServices.CreateScope();
            var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
            var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

            var gateway = new Gateway("SN", "RN", "127.0.0.1");
            peripheralDeviceRepository.Add(gateway.AddPeripheralDevice(sqlGuidGenerator.NewGuid(), "V1", PeripheralDeviceStatusValues.Online));
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
        public async Task GatewaySerialNumberNull()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayUpdateCommand()
            {
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "sn",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var result = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            var error = result.Errors.SingleOrDefault();

            Assert.AreEqual(GatewayMessages.SerialNumberIncorrectComposition.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.SerialNumber), error.PropertyName);
        }

        [Test]
        public async Task GatewaySerialNumberNotFound()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN1",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
            var validatoResult = await validator.ValidateAsync(query, default);

            Assert.IsNotNull(validatoResult);
            Assert.IsFalse(validatoResult.IsValid);
            Assert.AreEqual(1, validatoResult.Errors.Count);

            var error = validatoResult.Errors.SingleOrDefault();

            Assert.AreEqual(GeneralMessages.NotFound.GetDescription(), error.ErrorMessage);
            Assert.AreEqual(nameof(Gateway.SerialNumber), error.PropertyName);
        }

        [Test]
        public async Task GatewayReadableNameNull()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
            var peripheralDeviceStatusQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<PeripheralDeviceStatus>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNNRNNNNNNNNN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var peripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>();

            for (int i = 0; i < 6; i++)
            {
                peripheralDevices.Add(new GatewayUpdateCommand.PeripheralDeviceModel
                {
                    PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Offline,
                    Vendor = "V1"
                });
                peripheralDevices.Add(new GatewayUpdateCommand.PeripheralDeviceModel
                {
                    PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                    Vendor = "V2"
                });
            }

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = peripheralDevices
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var peripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
                {
                    new GatewayUpdateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = default,
                        Vendor = "V"
                    }
                }
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var peripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
                {
                    new GatewayUpdateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online
                    }
                }
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var peripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
                {
                    new GatewayUpdateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                        Vendor = ""
                    }
                }
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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

            var peripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>();

            var query = new GatewayUpdateCommand()
            {
                SerialNumber = "SN",
                IpAddress = "1.1.1.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>()
                {
                    new GatewayUpdateCommand.PeripheralDeviceModel
                    {
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online,
                        Vendor = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                    }
                }
            };
            var validator = new GatewayUpdateCommandValidator(gatewayQueryRepository, peripheralDeviceStatusQueryRepository);
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