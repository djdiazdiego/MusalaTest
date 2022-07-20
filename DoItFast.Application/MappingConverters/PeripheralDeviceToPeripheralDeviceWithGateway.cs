using AutoMapper;
using DoItFast.Application.Features.Dtos.Gateway;
using DoItFast.Domain.Models.GatewayAggregate;

namespace DoItFast.Application.MappingConverters
{
    public class PeripheralDeviceToPeripheralDeviceWithGateway : ITypeConverter<PeripheralDevice, PeripheralDeviceWithGatewayResponseDto>
    {
        public PeripheralDeviceWithGatewayResponseDto Convert(PeripheralDevice source, PeripheralDeviceWithGatewayResponseDto destination, ResolutionContext context)
        {
            if (destination == null)
                destination = new PeripheralDeviceWithGatewayResponseDto();

            destination.SerialNumber = source.Gateway.SerialNumber;
            destination.IpAddress = source.Gateway.IpAddress;
            destination.ReadableName = source.Gateway.ReadableName;
            destination.Id = source.Id;
            destination.Vendor = source.Vendor;
            destination.PeripheralDeviceStatusId = source.PeripheralDeviceStatusId;
            destination.Created = source.Created;

            return destination;
        }
    }
}
