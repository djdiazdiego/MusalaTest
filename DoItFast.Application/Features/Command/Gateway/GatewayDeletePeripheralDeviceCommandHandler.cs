using AutoMapper;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayDeletePeripheralDeviceCommandHandler : ICommandHandler<GatewayDeletePeripheralDeviceCommand, Response<PeripheralDeviceResponseDto>>
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
        public GatewayDeletePeripheralDeviceCommandHandler(
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

        public async Task<Response<PeripheralDeviceResponseDto>> Handle(GatewayDeletePeripheralDeviceCommand request, CancellationToken cancellationToken)
        {
            var gateway = await _gatewayRepository.FindAll()
                .Include(p => p.PeripheralDevices)
                .Where(p => p.Id == request.SerialNumber)
                .FirstAsync(cancellationToken);

            var peripheralDevice = DeletePeripheralDevice(gateway, request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<PeripheralDeviceResponseDto>(peripheralDevice);
            return new Response<PeripheralDeviceResponseDto>(response);
        }

        private Domain.Models.GatewayAggregate.PeripheralDevice DeletePeripheralDevice(Domain.Models.GatewayAggregate.Gateway gateway, GatewayDeletePeripheralDeviceCommand request)
        {
            var peripheralDeviceToTrack = gateway.RemovePeripheralDevice(request.Id);
            _deviceRepository.Remove(peripheralDeviceToTrack);
            return peripheralDeviceToTrack;
        }
    }
}
