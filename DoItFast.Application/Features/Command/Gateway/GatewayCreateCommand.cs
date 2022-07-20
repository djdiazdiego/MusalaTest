using DoItFast.Application.Features.Dtos.Gateway;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayCreateCommand : Command<GatewayResponseDto>
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

        public List<PeripheralDeviceModel> PeripheralDevices { get; set; }

        public class PeripheralDeviceModel : Gateway.PeripheralDeviceModel
        {
        }
    }
}
