using AutoMapper;
using DoItFast.Application.Features.Dtos;
using DoItFast.Domain.Core.Abstractions.Persistence;

namespace DoItFast.Application.Features.Queries.PeripheralDeviceStatus
{
    public sealed class PeripheralDeviceStatusGetAllQueryHandler : GetAllQueryHandler<Domain.Models.GatewayAggregate.PeripheralDeviceStatus, EnumerationDto, PeripheralDeviceStatusGetAllQuery>
    {
        public PeripheralDeviceStatusGetAllQueryHandler(IQueryRepository<Domain.Models.GatewayAggregate.PeripheralDeviceStatus> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
