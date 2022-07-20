using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Attributes;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    [FullMap(typeof(Domain.Models.GatewayAggregate.Gateway), ReverseMap = true)]
    public class GatewayResponseDto : IDto
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

        public List<PeripheralDeviceResponseDto> PeripheralDevices { get; set; }
    }
}
