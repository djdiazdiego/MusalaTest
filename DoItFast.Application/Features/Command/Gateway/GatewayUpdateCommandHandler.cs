using AutoMapper;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayUpdateCommandHandler : ICommandHandler<GatewayUpdateCommand, Response<GatewayResponseDto>>
    {
        private readonly IRepository<Domain.Models.GatewayAggregate.Gateway> _gatewayRepository;
        private readonly IRepository<Domain.Models.GatewayAggregate.PeripheralDevice> _deviceRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISqlGuidGenerator _sqlGuidGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gatewayRepository"></param>
        /// <param name="deviceRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="sqlGuidGenerator"></param>
        public GatewayUpdateCommandHandler(
            IRepository<Domain.Models.GatewayAggregate.Gateway> gatewayRepository,
            IRepository<Domain.Models.GatewayAggregate.PeripheralDevice> deviceRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ISqlGuidGenerator sqlGuidGenerator)
        {
            _gatewayRepository = gatewayRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _sqlGuidGenerator = sqlGuidGenerator;
        }

        public async Task<Response<GatewayResponseDto>> Handle(GatewayUpdateCommand request, CancellationToken cancellationToken)
        {
            var gateway = await _gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices)
                .Where(p => p.Id == request.SerialNumber)
                .FirstAsync(cancellationToken);

            UpdateGateway(gateway, request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<GatewayResponseDto>(gateway);
            return new Response<GatewayResponseDto>(response);
        }

        private void UpdateGateway(Domain.Models.GatewayAggregate.Gateway gateway, GatewayUpdateCommand request)
        {
            gateway.UpdateGateway(request.ReadableName, request.IpAddress);

            if (request.PeripheralDevices != null)
            {
                var devicesToRemove = gateway.PeripheralDevices.Where(p => !request.PeripheralDevices.Any(d => d.Id == p.Id))
                    .ToArray();

                foreach (var device in devicesToRemove)
                    gateway.RemovePeripheralDevice(device);

                var devicesToUpdate = new List<Domain.Models.GatewayAggregate.PeripheralDevice>();
                foreach (var deviceRequest in request.PeripheralDevices)
                {
                    var device = gateway.UpdatePeripheralDevice(deviceRequest.Id, deviceRequest.Vendor, deviceRequest.PeripheralDeviceStatusId);
                    if (device != null)
                        devicesToUpdate.Add(device);
                }

                var devicesToAdd = new List<Domain.Models.GatewayAggregate.PeripheralDevice>();
                var devicesRequestToAdd = request.PeripheralDevices
                    .Where(p => !gateway.PeripheralDevices.Any(r => r.Id == p.Id))
                    .ToArray();

                foreach (var device in devicesRequestToAdd)
                {
                    var deviceToAdd = gateway.AddPeripheralDevice(_sqlGuidGenerator.NewGuid(), device.Vendor, device.PeripheralDeviceStatusId);
                    devicesToAdd.Add(deviceToAdd);
                }

                if (devicesToRemove.Length > 0)
                    _deviceRepository.RemoveRange(devicesToRemove.ToArray());
                if (devicesToUpdate.Count > 0)
                    _deviceRepository.UpdateRange(devicesToUpdate.ToArray());
                if (devicesToAdd.Count > 0)
                    _deviceRepository.AddRange(devicesToAdd.ToArray());
            }

            _gatewayRepository.Update(gateway);
        }
    }
}
