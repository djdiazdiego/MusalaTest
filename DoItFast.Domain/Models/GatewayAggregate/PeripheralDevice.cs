using DoItFast.Domain.Core.Abstractions.Entities;
using System;

namespace DoItFast.Domain.Models.GatewayAggregate
{
    public class PeripheralDevice : Entity<Guid, Guid?>
    {
        private string _vendor;
        private PeripheralDeviceStatusValues _peripheralDeviceStatusId;
        private string _gatewayId;

        protected PeripheralDevice() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vendor"></param>
        /// <param name="peripheralDeviceStatusId"></param>
        /// <param name="gatewayId"></param>
        public PeripheralDevice(Guid id, string vendor, PeripheralDeviceStatusValues peripheralDeviceStatusId, string gatewayId):base(id)
        {
            _vendor = vendor;
            _peripheralDeviceStatusId = peripheralDeviceStatusId;
            _gatewayId = gatewayId;
        }

        /// <summary>
        /// Vendor.
        /// </summary>
        public string Vendor => _vendor;
        /// <summary>
        /// Status.
        /// </summary>
        public PeripheralDeviceStatusValues PeripheralDeviceStatusId => _peripheralDeviceStatusId;
        /// <summary>
        /// Foreign key to Gateway.
        /// </summary>
        public string GatewayId => _gatewayId;

        #region navigation properties
        /// <summary>
        /// Gateway navigation reference.
        /// </summary>
        public Gateway Gateway { get; set; }

        /// <summary>
        /// Peripheral Device Status navigation reference.
        /// </summary>
        public PeripheralDeviceStatus PeripheralDeviceStatus { get; set; }

        #endregion

        /// <summary>
        /// Update Peripheral Device
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="peripheralDeviceStatusId"></param>
        public void Update(string vendor, PeripheralDeviceStatusValues peripheralDeviceStatusId)
        {
            _vendor = vendor;
            _peripheralDeviceStatusId = peripheralDeviceStatusId;
        }
    }
}
