using AutoMapper;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using DoItFast.Test.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test
{
    public class TestingGateway
    {
        private SetupServices _setupServices;

        [SetUp]
        public void Setup()
        {
            _setupServices = new SetupServices();
        }

        [Test]
        public async Task CreateGateway()
        {
            using var scope = _setupServices.Provider.CreateScope();
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

            var gateway = await gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices.OrderBy(pd => pd.Vendor))
                .FirstOrDefaultAsync(default);

            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(gateway != null);
            Assert.IsTrue(gateway?.SerialNumber == commad.SerialNumber);
            Assert.IsTrue(gateway?.ReadableName == commad.ReadableName);
            Assert.IsTrue(gateway?.IpAddress == commad.IpAddress);
            Assert.IsTrue(gateway?.SerialNumber == commad.SerialNumber);
            Assert.IsTrue(gateway?.PeripheralDevices.Count == 2);
            Assert.IsTrue(gateway?.PeripheralDevices?.First()?.Vendor == "V1");
            Assert.IsTrue(gateway?.PeripheralDevices?.First()?.PeripheralDeviceStatusId == PeripheralDeviceStatusValues.Online);
        }

        [Test]
        public async Task UpdateGateway()
        {
            using var scope = _setupServices.Provider.CreateScope();
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
            var gateway = await gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices).FirstOrDefaultAsync(default);

            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(gateway != null);
            Assert.IsTrue(gateway?.IpAddress == updateCommad.IpAddress);
            Assert.IsTrue(gateway?.ReadableName == updateCommad.ReadableName);
            Assert.IsTrue(gateway?.PeripheralDevices.Count == 1);
            Assert.IsTrue(gateway?.PeripheralDevices?.First()?.PeripheralDeviceStatusId == PeripheralDeviceStatusValues.Offline);
            Assert.IsTrue(gateway?.PeripheralDevices?.First()?.Vendor == "V2");
        }
    }
}