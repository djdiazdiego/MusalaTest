using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.Features.Command.Gateway
{
    public sealed class GatewayUpdatePeripheralDeviceCommand : PeripheralDeviceToUpdateDeleteModel, ICommand<Response<PeripheralDeviceResponseDto>>
    {
        /// <summary>
        /// Vendor
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public PeripheralDeviceStatusValues PeripheralDeviceStatusId { get; set; }
    }
}
