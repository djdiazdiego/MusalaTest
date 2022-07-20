using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayDeletePeripheralDeviceCommand : PeripheralDeviceToUpdateDeleteModel, ICommand<Response<PeripheralDeviceResponseDto>>
    {
    }
}
