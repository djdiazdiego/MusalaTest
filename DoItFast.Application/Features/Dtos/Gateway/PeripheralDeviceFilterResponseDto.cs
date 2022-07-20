using DoItFast.Domain.Core.Abstractions.Dtos;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    public class PeripheralDeviceFilterResponseDto : FilterResponseDto<PeripheralDeviceWithGatewayResponseDto>, IDto
    {
        public PeripheralDeviceFilterResponseDto(List<PeripheralDeviceWithGatewayResponseDto> entities, int total) : base(entities, total)
        {
        }
    }
}
