using DoItFast.Application.Features.ValidatorExtensions;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using FluentValidation;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayUpdatePeripheralDeviceCommandValidator : BaseValidator<GatewayUpdatePeripheralDeviceCommand>
    {
        public GatewayUpdatePeripheralDeviceCommandValidator(IQueryRepository<Domain.Models.GatewayAggregate.Gateway> queryRepository,
            IQueryRepository<PeripheralDeviceStatus> deviceStatusQueryRepository)
        {
            RuleFor(p => p)
                .ValidatePeripheralDeviceToUpdateDelete(queryRepository)
                .OverridePropertyName(nameof(PeripheralDevice));

            RuleFor(p => p.PeripheralDeviceStatusId)
                .ValidatePeripheralDeviceStatusId(deviceStatusQueryRepository);

            RuleFor(p => p.Vendor)
                .ValidateVendor();
        }
    }
}
