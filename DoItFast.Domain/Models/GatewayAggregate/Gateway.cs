using DoItFast.Domain.Core.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DoItFast.Domain.Models.GatewayAggregate
{
    public class Gateway : AggregateRoot<string, Guid?>
    {
        private readonly List<PeripheralDevice> _peripheralDevices;
        private string _readableName;
        private string _ipAddress;

        protected Gateway()
        {
            _peripheralDevices = new List<PeripheralDevice>();
        }

        public Gateway(string serialNumber, string readableName, string ipAddress) : base(serialNumber)
        {
            _readableName = readableName;
            _ipAddress = ipAddress;
            _peripheralDevices = new List<PeripheralDevice>();
        }

        /// <summary>
        /// Serial number
        /// </summary>
        [NotMapped]
        public string SerialNumber => this.Id;
        /// <summary>
        /// Human readable name
        /// </summary>
        public string ReadableName => _readableName;
        /// <summary>
        /// Ip address
        /// </summary>
        public string IpAddress => _ipAddress;

        #region navigation properties

        /// <summary>
        /// PeripheralDevices navigation reference.
        /// </summary>
        public IReadOnlyCollection<PeripheralDevice> PeripheralDevices => _peripheralDevices;

        #endregion

        public void UpdateGateway(string readableName, string ipAddress)
        {
            _readableName = readableName;
            _ipAddress = ipAddress;
        }

        /// <summary>
        /// Add Peripheral Device.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vendor"></param>
        /// <param name="peripheralDeviceStatusId"></param>
        public PeripheralDevice AddPeripheralDevice(Guid id, string vendor, PeripheralDeviceStatusValues peripheralDeviceStatusId)
        {
            var peripheralDevice = new PeripheralDevice(id, vendor, peripheralDeviceStatusId, this.Id);
            _peripheralDevices.Add(peripheralDevice);

            return peripheralDevice;
        }

        /// <summary>
        /// Update Peripheral Device.
        /// </summary>
        /// <param name="peripheralDeviceId"></param>
        /// <param name="vendor"></param>
        /// <param name="peripheralDeviceStatusId"></param>
        public PeripheralDevice UpdatePeripheralDevice(Guid peripheralDeviceId, string vendor, PeripheralDeviceStatusValues peripheralDeviceStatusId)
        {
            var peripheralDevice = _peripheralDevices.FirstOrDefault(p => p.Id == peripheralDeviceId);

            if (peripheralDevice != null)
            {
                peripheralDevice.Update(vendor, peripheralDeviceStatusId);
                return peripheralDevice;
            }
            return null;
        }

        /// <summary>
        /// Remove Peripheral Device.
        /// </summary>
        /// <param name="peripheralDeviceId"></param>
        public PeripheralDevice RemovePeripheralDevice(Guid peripheralDeviceId)
        {
            var peripheralDevice = _peripheralDevices.FirstOrDefault(p => p.Id == peripheralDeviceId);

            if (peripheralDevice != null)
            {
                var peripheralDeviceToReturn = peripheralDevice;
                _peripheralDevices.Remove(peripheralDevice);
                return peripheralDeviceToReturn;
            }

            return null;
        }

        /// <summary>
        /// Remove Peripheral Device
        /// </summary>
        /// <param name="peripheralDevice"></param>
        public void RemovePeripheralDevice(PeripheralDevice peripheralDevice) =>
            _peripheralDevices.Remove(peripheralDevice);

        /// <summary>
        /// Clear Peripheral Devices
        /// </summary>
        public void CleanPeripheralDevices() => _peripheralDevices.Clear();
    }
}
