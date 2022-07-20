using DoItFast.Application.Features.Dtos;
using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.Features.Queries.PeripheralDeviceStatus
{
    public class PeripheralDeviceStatusGetQuery : Query<PeripheralDeviceStatusValues, EnumerationDto>
    {
        public PeripheralDeviceStatusGetQuery(PeripheralDeviceStatusValues id) : base(id)
        {
        }
    }
}
