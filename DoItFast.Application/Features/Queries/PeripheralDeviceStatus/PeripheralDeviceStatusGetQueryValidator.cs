using DoItFast.Application.Extensions;
using DoItFast.Domain.Core.Abstractions.Persistence;

namespace DoItFast.Application.Features.Queries.PeripheralDeviceStatus
{
    public sealed class PeripheralDeviceStatusGetQueryValidator : BaseValidator<PeripheralDeviceStatusGetQuery>
    {
        public PeripheralDeviceStatusGetQueryValidator(IQueryRepository<Domain.Models.GatewayAggregate.PeripheralDeviceStatus> queryRepository)
        {
            RuleFor(p => p.Id)
                .ApiAlreadyExists(queryRepository);
        }
    }
}
