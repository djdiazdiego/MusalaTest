using AutoMapper;
using DoItFast.Application.Features.Dtos;
using DoItFast.Domain.Core.Abstractions.Persistence;

namespace DoItFast.Application.Features.Queries.PeripheralDeviceStatus
{
    public class PeripheralDeviceStatusGetQueryHandler : GetQueryHandler<Domain.Models.GatewayAggregate.PeripheralDeviceStatus, EnumerationDto, PeripheralDeviceStatusGetQuery>
    {
        public PeripheralDeviceStatusGetQueryHandler(IQueryRepository<Domain.Models.GatewayAggregate.PeripheralDeviceStatus> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
