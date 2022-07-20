using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class PeripheralDeviceModel
    {
        /// <summary>
        /// Vendor.
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Status.
        /// </summary>
        public PeripheralDeviceStatusValues PeripheralDeviceStatusId { get; set; }
    }

    public class PeripheralDeviceToUpdateDeleteModel
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
