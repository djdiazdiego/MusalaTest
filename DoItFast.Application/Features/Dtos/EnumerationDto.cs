using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Attributes;
using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.Features.Dtos
{
    [FullMap(typeof(PeripheralDeviceStatus), ReverseMap = true)]
    public class EnumerationDto: IDto
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the field.
        /// </summary>
        public string Name { get; set; }
    }
}
