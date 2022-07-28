using AutoMapper;
using DoItFast.Application.Extensions;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using DoItFast.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test.CommandTests
{
    public class GatewayUpdatePeripheralDeviceCommandTest
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
        public async Task DeletePeripheralDevice()
        {
            using var scope = _setupServices.CreateScope();
            var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
            var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
            var mapper = scope.ServiceProvider.GetService<IMapper>();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

            var commad = new GatewayCreateCommand
            {
                SerialNumber = "SN",
                IpAddress = "127.0.0.1",
                ReadableName = "RN",
                PeripheralDevices = new System.Collections.Generic.List<GatewayCreateCommand.PeripheralDeviceModel>
                {
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        Vendor = "V1",
                        PeripheralDeviceStatusId= PeripheralDeviceStatusValues.Online
                    },
                    new GatewayCreateCommand.PeripheralDeviceModel
                    {
                        Vendor = "V2",
                        PeripheralDeviceStatusId= PeripheralDeviceStatusValues.Offline
                    }
                }
            };
            var commandHandler = new GatewayCreateCommandHandler(gatewayRepository, peripheralDeviceRepository, mapper, unitOfWork, sqlGuidGenerator);

            await commandHandler.Handle(commad, default);

            var gateway = await gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices).FirstOrDefaultAsync(default);

            Assert.IsNotNull(gateway);

            var peripheralDevice = gateway.PeripheralDevices.First();
            var id = peripheralDevice.Id;
            var status = peripheralDevice.PeripheralDeviceStatusId;
            var vendor = peripheralDevice.Vendor;

            var updateCommand = new GatewayUpdatePeripheralDeviceCommand
            {
                Id = id,
                SerialNumber = "SN",
                PeripheralDeviceStatusId = status,
                Vendor = vendor
            };
            var commandUpdateHandler = new GatewayUpdatePeripheralDeviceCommandHandler(gatewayRepository, peripheralDeviceRepository, mapper, unitOfWork);
            await commandUpdateHandler.Handle(updateCommand, default);

            gateway = await gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices).FirstOrDefaultAsync(default);
            peripheralDevice = gateway?.PeripheralDevices?.First(p => p.Id == id);

            Assert.IsNotNull(gateway);
            Assert.IsNotNull(peripheralDevice);
            Assert.AreEqual(status, peripheralDevice.PeripheralDeviceStatusId);
            Assert.AreEqual(vendor, peripheralDevice.Vendor);
        }
    }
}