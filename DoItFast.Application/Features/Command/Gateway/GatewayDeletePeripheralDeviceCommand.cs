using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;

namespace DoItFast.Application.Features.Command.Gateway
{
    public sealed class GatewayDeletePeripheralDeviceCommand : PeripheralDeviceToUpdateDeleteModel, ICommand<Response<PeripheralDeviceResponseDto>>
    {
    }
}
