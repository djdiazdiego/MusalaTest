using AutoMapper;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayUpdatePeripheralDeviceCommandHanadler : ICommandHandler<GatewayUpdatePeripheralDeviceCommand, Response<PeripheralDeviceResponseDto>>
    {
        private readonly IRepository<Domain.Models.GatewayAggregate.Gateway> _gatewayRepository;
        private readonly IRepository<Domain.Models.GatewayAggregate.PeripheralDevice> _deviceRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gatewayRepository"></param>
        /// <param name="deviceRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        public GatewayUpdatePeripheralDeviceCommandHanadler(
            IRepository<Domain.Models.GatewayAggregate.Gateway> gatewayRepository,
            IRepository<Domain.Models.GatewayAggregate.PeripheralDevice> deviceRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _gatewayRepository = gatewayRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<PeripheralDeviceResponseDto>> Handle(GatewayUpdatePeripheralDeviceCommand request, CancellationToken cancellationToken)
        {
            var gateway = await _gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices)
                .Where(p => p.Id == request.SerialNumber)
                .FirstAsync(cancellationToken);

            var peripheralDevice = UpdatePeripheralDevice(gateway, request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<PeripheralDeviceResponseDto>(peripheralDevice);
            return new Response<PeripheralDeviceResponseDto>(response);
        }

        private Domain.Models.GatewayAggregate.PeripheralDevice UpdatePeripheralDevice(Domain.Models.GatewayAggregate.Gateway gateway, GatewayUpdatePeripheralDeviceCommand request)
        {
            var peripheralDeviceToTrack = gateway.UpdatePeripheralDevice(request.Id, request.Vendor, request.PeripheralDeviceStatusId);
            _deviceRepository.Update(peripheralDeviceToTrack);
            return peripheralDeviceToTrack;
        }
    }
}
