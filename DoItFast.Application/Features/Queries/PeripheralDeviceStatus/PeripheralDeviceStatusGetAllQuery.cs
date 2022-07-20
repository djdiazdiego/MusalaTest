using DoItFast.Application.Features.Dtos;
using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.Features.Queries.PeripheralDeviceStatus
{
    public sealed class PeripheralDeviceStatusGetAllQuery : EnumerationQuery<PeripheralDeviceStatusValues, EnumerationDto[]>
    {
    }
}
