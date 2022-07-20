using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Attributes;
using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    [FullMap(typeof(GatewayUpdateCommand.PeripheralDeviceModel))]
    public class PeripheralDeviceUpdateRequestDto : IDto
    {
        /// <summary>
        /// Peripheral Device identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Vendor.
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        public PeripheralDeviceStatusValues PeripheralDeviceStatusId { get; set; }
    }
}
