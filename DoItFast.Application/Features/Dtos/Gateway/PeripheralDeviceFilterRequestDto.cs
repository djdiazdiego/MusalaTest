using DoItFast.Application.Features.Queries.PeripheralDevice;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Attributes;

namespace DoItFast.Application.Features.Dtos.Gateway
{
    [FullMap(typeof(PeripheralDeviceFilterQuery))]
    public class PeripheralDeviceFilterRequestDto : Filter, IDto
    {
       
    }
}


