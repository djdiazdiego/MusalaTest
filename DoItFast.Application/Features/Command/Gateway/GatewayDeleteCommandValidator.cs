using DoItFast.Application.Extensions;
using DoItFast.Domain.Core.Abstractions.Persistence;

namespace DoItFast.Application.Features.Command.Gateway
{
    public class GatewayDeleteCommandValidator : BaseValidator<GatewayDeleteCommand>
    {
        public GatewayDeleteCommandValidator(IQueryRepository<Domain.Models.GatewayAggregate.Gateway> queryRepository)
        {
            RuleFor(p => p.Id)
                .ApiAlreadyExists(queryRepository);
        }
    }
}
