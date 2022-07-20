using DoItFast.Domain.Core.Abstractions.Dtos;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    public class GatewayFilterResponseDto : FilterResponseDto<GatewayResponseDto>, IDto
    {
        public GatewayFilterResponseDto(List<GatewayResponseDto> entities, int total) : base(entities, total)
        {
        }
    }
}
