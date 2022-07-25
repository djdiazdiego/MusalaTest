using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace DoItFast.Test.Setup
{
    public static class SetupGateway
    {
        public static Task AddGatewayAsync(this DbContextWrite dbContext, Gateway[] gateways, CancellationToken cancellationToken)
        {
            dbContext.Set<Gateway>().AddRange(gateways);
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
