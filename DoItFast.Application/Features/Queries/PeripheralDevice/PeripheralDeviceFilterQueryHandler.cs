using AutoMapper;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Domain.Core.Abstractions.Persistence;

namespace DoItFast.Application.Features.Queries.PeripheralDevice
{
    public sealed class PeripheralDeviceFilterQueryHandler : FilterQueryHandler<Domain.Models.GatewayAggregate.PeripheralDevice,
        PeripheralDeviceFilterResponseDto,
        PeripheralDeviceWithGatewayResponseDto,
        PeripheralDeviceFilterQuery>
    {
        public PeripheralDeviceFilterQueryHandler(IQueryRepository<Domain.Models.GatewayAggregate.PeripheralDevice> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
