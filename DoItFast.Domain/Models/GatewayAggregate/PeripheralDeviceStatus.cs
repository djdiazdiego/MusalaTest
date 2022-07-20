using DoItFast.Domain.Core.Abstractions.Entities;
using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using System;

namespace DoItFast.Domain.Models.GatewayAggregate
{
    /// <summary>
    /// 
    /// </summary>
    public class PeripheralDeviceStatus : Enumeration<PeripheralDeviceStatusValues, Guid?>, INotRepository
    {
        /// <summary>
        /// 
        /// </summary>
        protected PeripheralDeviceStatus() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PeripheralDeviceStatusId"></param>
        /// <param name="name"></param>
        public PeripheralDeviceStatus(PeripheralDeviceStatusValues PeripheralDeviceStatusId, string name) : base(PeripheralDeviceStatusId, name) { }
    }
}
