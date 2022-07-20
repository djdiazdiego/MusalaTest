using DoItFast.Application.Extensions;
using DoItFast.Application.Features.ValidatorExtensions;
using DoItFast.Domain.Core.Abstractions.Persistence;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayCreateCommandValidator : BaseValidator<GatewayCreateCommand>
    {
        public GatewayCreateCommandValidator(IQueryRepository<Domain.Models.GatewayAggregate.Gateway> queryRepository,
            IQueryRepository<Domain.Models.GatewayAggregate.PeripheralDeviceStatus> deviceStatusQueryRepository) : base()
        {
            RuleFor(p => p.SerialNumber)
                .ValidateSerialNumber()
                .ApiDoesNotExist(queryRepository);

            RuleFor(p => p.ReadableName)
                .ValidateReadableName();

            RuleFor(p => p.IpAddress)
                .ValidateIpAddress();

            RuleFor(p => p.PeripheralDevices)
                .ValidatePeripheralDevices();

            RuleForEach(p => p.PeripheralDevices)
                .ValidatePeripheralDevicesElements(deviceStatusQueryRepository);
        }
    }
}
