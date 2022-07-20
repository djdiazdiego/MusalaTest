using DoItFast.Application.MappingConverters;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Attributes;
using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    [CustomMap(typeof(PeripheralDevice), typeof(PeripheralDeviceToPeripheralDeviceWithGateway), reverseMap: true)]
    public class PeripheralDeviceWithGatewayResponseDto : IDto
    {
        /// <summary>
        /// Peripheral Device identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Vendor
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public PeripheralDeviceStatusValues PeripheralDeviceStatusId { get; set; }
        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset Created { get; set; }
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
    }
}
