using AutoMapper;
using DoItFast.Application.Exceptions;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Test.Setup;
using DoItFast.WebApi.Controllers.V1;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoItFast.Test.IntegrationTests
{
    public class GatewayControllerTest
    {
        private SetupServices _setupServices;

        [SetUp]
        public async Task Setup()
        {
            _setupServices = new SetupServices();
        }

        [Test]
        public async Task CreateGatewayOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                var dto = new GatewayCreateRequestDto
                {
                    SerialNumber = "SN",
                    IpAddress = "127.0.0.1",
                    ReadableName = "RN",
                    PeripheralDevices = new List<PeripheralDeviceCreateRequestDto>()
                {
                    new PeripheralDeviceCreateRequestDto
                    {
                        Vendor= "V1",
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online
                    },
                    new PeripheralDeviceCreateRequestDto
                    {
                        Vendor= "V2",
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Offline
                    }
                }
                };
                var controller = new GatewayController(mediator, mapper);
                var response = await controller.Post(dto, default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<GatewayResponseDto>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task CreateGatewayValidationError()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                var dto = new GatewayCreateRequestDto
                {
                    SerialNumber = "sn"
                };

                var controller = new GatewayController(mediator, mapper);

                Assert.CatchAsync<ValidationException>(async () => await controller.Post(dto, default));
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task PageGatewayOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                var dto = new GatewayFilterRequestDto
                {
                    Order = new ColumnNameModel { SortBy = "", SortOperation = Domain.Core.Enums.SortOperation.ASC },
                    Paging = new PagingModel { Page = 1, PageSize = 10 }
                };

                var controller = new GatewayController(mediator, mapper);
                var response = await controller.Page(dto, default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<GatewayFilterResponseDto>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task UpdateGatewayOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();

                var controller = new GatewayController(mediator, mapper);
                var dto = new GatewayCreateRequestDto
                {
                    SerialNumber = "SN",
                    IpAddress = "127.0.0.1",
                    ReadableName = "RN",
                    PeripheralDevices = new List<PeripheralDeviceCreateRequestDto>()
                {
                    new PeripheralDeviceCreateRequestDto
                    {
                        Vendor= "V1",
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Online
                    },
                    new PeripheralDeviceCreateRequestDto
                    {
                        Vendor= "V2",
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Offline
                    }
                }
                };
                await controller.Post(dto, default);

                var gateway = await gatewayQueryRepository.FindAll()
                    .Include(p => p.PeripheralDevices)
                    .SingleOrDefaultAsync(default);
                var peripheralDevice = gateway?.PeripheralDevices?.First();

                Assert.IsNotNull(gateway);
                Assert.IsNotNull(peripheralDevice);

                var updatedto = new GatewayUpdateRequestDto
                {
                    SerialNumber = "SN",
                    IpAddress = "127.0.0.2",
                    ReadableName = "RN",
                    PeripheralDevices = new List<PeripheralDeviceUpdateRequestDto>()
                {
                    new PeripheralDeviceUpdateRequestDto
                    {
                        Id = peripheralDevice.Id,
                        Vendor= "V3",
                        PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Offline
                    }
                }
                };
                var response = await controller.Put(updatedto, default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<GatewayResponseDto>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task UpdateGatewayValidationError()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                var dto = new GatewayCreateRequestDto
                {
                    SerialNumber = "sn"
                };

                var controller = new GatewayController(mediator, mapper);

                Assert.CatchAsync<ValidationException>(async () => await controller.Post(dto, default));
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }
    }
}