using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Attributes;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    [FullMap(typeof(GatewayDeletePeripheralDeviceCommand))]
    public class GatewayDeletePeripheralDeviceRequestDto : IDto
    {
        /// <summary>
        /// Gateway serial number
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Peripheral Device identifier
        /// </summary>
        public Guid Id { get; set; }
    }
}
