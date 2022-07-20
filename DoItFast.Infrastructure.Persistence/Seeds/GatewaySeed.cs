using DoItFast.Domain.Models.GatewayAggregate;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DoItFast.Infrastructure.Persistence.Seeds
{
    public sealed class GatewaySeed : DataSeed<Gateway>
    {
        public override async Task SeedAsync(IServiceProvider provider, CancellationToken cancelationToken)
        {
            using var scope = provider.CreateScope();
            var sqlGuidGenerator = scope.ServiceProvider.GetRequiredService<ISqlGuidGenerator>();
            var peripheralDevices = new List<Gateway>();

            for (int i = 0; i < 100; i++)
            {
                var gateway = new Gateway($"SN{i}", $"RN{i}", $"1.1.1.{i}");
                gateway.AddPeripheralDevice(sqlGuidGenerator.NewGuid(), $"V{i}", PeripheralDeviceStatusValues.Online);
                gateway.AddPeripheralDevice(sqlGuidGenerator.NewGuid(), $"V{i + 1}", PeripheralDeviceStatusValues.Offline);
                peripheralDevices.Add(gateway);
            }

            await this.SaveChangesAsync(provider, peripheralDevices.ToArray(), cancelationToken);
        }
    }
}
