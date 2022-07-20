using DoItFast.Application.Features.Command.Gateway;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Attributes;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    [FullMap(typeof(GatewayUpdateCommand))]
    public class GatewayUpdateRequestDto : IDto
    {
        /// <summary>
        /// Serial number
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Human readable name
        /// </summary>
        public string ReadableName { get; set; }
        /// <summary>
        /// Ip address
        /// </summary>
        public string IpAddress { get; set; }

        public List<PeripheralDeviceUpdateRequestDto> PeripheralDevices { get; set; }        
    }
}
