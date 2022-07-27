using AutoMapper;
using DoItFast.Application.Exceptions;
using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Application.Features.Dtos;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
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
    public class PeripheralDeviceControllerTest
    {
        private SetupServices _setupServices;

        [SetUp]
        public async Task Setup()
        {
            _setupServices = new SetupServices();
        }

        [Test]
        public async Task PagePeripheralDeviceOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                var dto = new PeripheralDeviceFilterRequestDto
                {
                    Order = new ColumnNameModel { SortBy = "", SortOperation = Domain.Core.Enums.SortOperation.ASC },
                    Paging = new PagingModel { Page = 1, PageSize = 10 }
                };

                var controller = new PeripheralDeviceController(mediator, mapper);
                var response = await controller.Page(dto, default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<PeripheralDeviceFilterResponseDto>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task GetAllPeripheralDeviceOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();

                var controller = new PeripheralDeviceController(mediator, mapper);
                var response = await controller.GetAll(default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<EnumerationDto[]>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task GetPeripheralDeviceOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
                var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
                var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();
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

                var gateway = await gatewayQueryRepository.FindAll()
                    .Include(p => p.PeripheralDevices)
                    .SingleOrDefaultAsync(default);
                var peripheralDevice = gateway?.PeripheralDevices?.First();

                Assert.IsNotNull(gateway);
                Assert.IsNotNull(peripheralDevice);

                var controller = new PeripheralDeviceController(mediator, mapper);
                var response = await controller.Get(peripheralDevice.PeripheralDeviceStatusId, default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<EnumerationDto>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task GetPeripheralDeviceValidationError()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var mapper = scope.ServiceProvider.GetService<IMapper>();

                var controller = new PeripheralDeviceController(mediator, mapper);

                Assert.CatchAsync<ValidationException>(async () => await controller.Get(default, default));
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task UpdatePeripheralDeviceOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
                var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
                var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();
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

                var gateway = await gatewayQueryRepository.FindAll()
                    .Include(p => p.PeripheralDevices)
                    .SingleOrDefaultAsync(default);
                var peripheralDevice = gateway?.PeripheralDevices?.First();

                Assert.IsNotNull(gateway);
                Assert.IsNotNull(peripheralDevice);

                var controller = new PeripheralDeviceController(mediator, mapper);
                var updatedto = new GatewayUpdatePeripheralDeviceRequestDto
                {
                    SerialNumber = "SN",
                    Id = peripheralDevice.Id,
                    PeripheralDeviceStatusId = peripheralDevice.PeripheralDeviceStatusId == PeripheralDeviceStatusValues.Online ?
                        PeripheralDeviceStatusValues.Offline : PeripheralDeviceStatusValues.Online,
                    Vendor = "V3"
                };
                var response = await controller.UpdatePeripheralDevice(updatedto, default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<PeripheralDeviceResponseDto>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task UpdatePeripheralDeviceValidationError()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

                var controller = new PeripheralDeviceController(mediator, mapper);
                var dto = new GatewayUpdatePeripheralDeviceRequestDto
                {
                    SerialNumber = "SN",
                    Id = sqlGuidGenerator.NewGuid(),
                    PeripheralDeviceStatusId = PeripheralDeviceStatusValues.Offline,
                    Vendor = "V3"
                };

                Assert.CatchAsync<ValidationException>(async () => await controller.UpdatePeripheralDevice(dto, default));
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task DeletePeripheralDeviceOk()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var gatewayRepository = scope.ServiceProvider.GetService<IRepository<Gateway>>();
                var gatewayQueryRepository = scope.ServiceProvider.GetService<IQueryRepository<Gateway>>();
                var peripheralDeviceRepository = scope.ServiceProvider.GetService<IRepository<PeripheralDevice>>();
                var mediator = scope.ServiceProvider.GetService<IMediator>();
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

                var gateway = await gatewayQueryRepository.FindAll()
                    .Include(p => p.PeripheralDevices)
                    .SingleOrDefaultAsync(default);
                var peripheralDevice = gateway?.PeripheralDevices?.First();

                Assert.IsNotNull(gateway);
                Assert.IsNotNull(peripheralDevice);

                var controller = new PeripheralDeviceController(mediator, mapper);
                var updatedto = new GatewayDeletePeripheralDeviceRequestDto
                {
                    SerialNumber = "SN",
                    Id = peripheralDevice.Id
                };
                var response = await controller.DeletePeripheralDevice(updatedto, default);

                if (response.Result is OkObjectResult okObject)
                    Assert.IsInstanceOf<Response<PeripheralDeviceResponseDto>>(okObject.Value);
                else
                    Assert.Fail();
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }

        [Test]
        public async Task DeletePeripheralDeviceValidationError()
        {
            try
            {
                using var scope = _setupServices.CreateScope();
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var mapper = scope.ServiceProvider.GetService<IMapper>();
                var sqlGuidGenerator = scope.ServiceProvider.GetService<ISqlGuidGenerator>();

                var controller = new PeripheralDeviceController(mediator, mapper);
                var dto = new GatewayDeletePeripheralDeviceRequestDto
                {
                    SerialNumber = "SN",
                    Id = sqlGuidGenerator.NewGuid()
                };

                Assert.CatchAsync<ValidationException>(async () => await controller.DeletePeripheralDevice(dto, default));
            }
            finally
            {
                await _setupServices.DisposeAsync();
            }
        }
    }
}