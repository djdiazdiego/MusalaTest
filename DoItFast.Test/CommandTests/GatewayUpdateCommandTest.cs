using AutoMapper;
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
    public class GatewayUpdateCommandTest
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
        public async Task UpdateGateway()
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
            await commandHandler.Handle(commad, default);

            var gateway = await gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices).FirstOrDefaultAsync(default);

            Assert.IsNotNull(gateway);

            var updateCommad = new GatewayUpdateCommand
            {
                SerialNumber = "SN",
                IpAddress = "127.0.0.2",
                ReadableName = "RN1",
                PeripheralDevices = new System.Collections.Generic.List<GatewayUpdateCommand.PeripheralDeviceModel>
                {
                    new GatewayUpdateCommand.PeripheralDeviceModel
                    {
                        Vendor= "V2",
                        PeripheralDeviceStatusId= PeripheralDeviceStatusValues.Offline
                    }
                }
            };
            var updateCommandHandler = new GatewayUpdateCommandHandler(gatewayRepository, peripheralDeviceRepository, mapper, unitOfWork, sqlGuidGenerator);
            var result = await updateCommandHandler.Handle(updateCommad, default);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);

            gateway = await gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices).FirstOrDefaultAsync(default);

            Assert.IsNotNull(gateway);
            Assert.AreEqual(updateCommad.IpAddress, gateway.IpAddress);
            Assert.AreEqual(updateCommad.ReadableName, gateway.ReadableName);
            Assert.AreEqual(1, gateway?.PeripheralDevices.Count);
            Assert.AreEqual(PeripheralDeviceStatusValues.Offline, gateway.PeripheralDevices.First().PeripheralDeviceStatusId);
            Assert.AreEqual("V2", gateway.PeripheralDevices.First().Vendor);
        }
    }
}