using AutoMapper;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Application.Features.Queries.PeripheralDevice;
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
    public class PeripheralDeviceFilterQueryTest
    {
        private SetupServices _setupServices;

        [SetUp]
        public async Task Setup()
        {
            _setupServices = new SetupServices();
        }

        [Test]
        public async Task FilterPeripheralDevice()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
                var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
                var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

                var commandHandler = new GatewayCreateCommandHandler(gatewayRepository, peripheralDeviceRepository, mapper, unitOfWork, sqlGuidGenerator);

                for (int i = 1; i < 12; i++)
                {
                    var commad = new GatewayCreateCommand
                    {
                        SerialNumber = $"SN{i}",
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

                    var result = await commandHandler.Handle(commad, default);
                    Assert.IsTrue(result.Succeeded);
                }

                var gateways = await gatewayQueryRepository.FindAll()
                    .Include(p => p.PeripheralDevices.OrderBy(pd => pd.Vendor))
                    .ToArrayAsync(default);

                Assert.NotNull(gateways);
                Assert.IsTrue(gateways.Length == 11);

                var query = new PeripheralDeviceFilterQuery
                {
                    Order = new Application.Wrappers.ColumnNameModel
                    {
                        SortBy = "SerialNumber",
                        SortOperation = Domain.Core.Enums.SortOperation.DESC
                    },
                    Paging = new Application.Wrappers.PagingModel
                    {
                        Page = 1,
                        PageSize = 10
                    }
                };
                var queryHandler = new PeripheralDeviceFilterQueryHandler(peripheralDeviceRepository, mapper);
                var queryResult = await queryHandler.Handle(query, default);

                Assert.NotNull(queryResult);
                Assert.IsTrue(queryResult.Succeeded);
                Assert.NotNull(queryResult.Data);
                Assert.AreEqual(22, queryResult.Data.Total);

                var data = queryResult.Data.Data;

                Assert.NotNull(data);
                Assert.AreEqual(10, data.Count);
                Assert.AreEqual("SN9", data[0].SerialNumber);
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

    }
}