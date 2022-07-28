using DoItFast.Application.Extensions;
using DoItFast.Domain.Core.Abstractions.Persistence;
using FluentValidation;

namespace DoItFast.Application.Features.Command.Gateway
{
    public sealed class GatewayDeleteCommandValidator : BaseValidator<GatewayDeleteCommand>
    {
        public GatewayDeleteCommandValidator(IQueryRepository<Domain.Models.GatewayAggregate.Gateway> queryRepository)
        {
            RuleFor(p => p.Id)
                .ApiAlreadyExists(queryRepository)
                .OverridePropertyName(nameof(Domain.Models.GatewayAggregate.Gateway.SerialNumber));
        }
    }
}
