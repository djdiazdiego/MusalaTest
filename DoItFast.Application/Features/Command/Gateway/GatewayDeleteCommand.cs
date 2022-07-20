using DoItFast.Application.Features.Dtos.Gateway;

namespace DoItFast.Application.Features.Command.Gateway
{
    public sealed class GatewayDeleteCommand : Command<string, GatewayResponseDto>
    {
        public GatewayDeleteCommand(string id) : base(id)
        {
        }
    }
}
