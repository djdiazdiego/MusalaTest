using DoItFast.Application.Features.ValidatorExtensions;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Models.GatewayAggregate;
using FluentValidation;

namespace DoItFast.Application.Features.Command.Gateway
{
    public sealed class GatewayDeletePeripheralDeviceCommandValidator : BaseValidator<GatewayDeletePeripheralDeviceCommand>
    {
        public GatewayDeletePeripheralDeviceCommandValidator(IQueryRepository<Domain.Models.GatewayAggregate.Gateway> queryRepository)
        {
            RuleFor(p => p).ValidatePeripheralDeviceToUpdateDelete(queryRepository)
                .OverridePropertyName(nameof(PeripheralDevice)); ;
        }
    }
}
