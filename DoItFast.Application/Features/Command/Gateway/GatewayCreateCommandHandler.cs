using AutoMapper;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Shared.Services.Interfaces;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayCreateCommandHandler : ICommandHandler<GatewayCreateCommand, Response<GatewayResponseDto>>
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
        public GatewayCreateCommandHandler(
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

        public async Task<Response<GatewayResponseDto>> Handle(GatewayCreateCommand request, CancellationToken cancellationToken)
        {
            var gateway = CreateGateway(request);
            _gatewayRepository.Add(gateway);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<GatewayResponseDto>(gateway);
            return new Response<GatewayResponseDto>(response);
        }

        private Domain.Models.GatewayAggregate.Gateway CreateGateway(GatewayCreateCommand request)
        {
            var gateway = new Domain.Models.GatewayAggregate.Gateway(request.SerialNumber, request.ReadableName, request.IpAddress);

            if (request.PeripheralDevices != null)
            {
                var devicesToTrack = new List<Domain.Models.GatewayAggregate.PeripheralDevice>();
                foreach (var deviceRequest in request.PeripheralDevices)
                {
                    var device = gateway.AddPeripheralDevice(_sqlGuidGenerator.NewGuid(), deviceRequest.Vendor, deviceRequest.PeripheralDeviceStatusId);
                    devicesToTrack.Add(device);
                }

                if (devicesToTrack.Count > 0)
                    _deviceRepository.AddRange(devicesToTrack.ToArray());
            }

            return gateway;
        }
    }
}
