using DoItFast.Domain.Models.GatewayAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoItFast.Infrastructure.Persistence.Configurations
{
    public class PeripheralDeviceStatusConfiguration : BaseEnumerationConfiguration<PeripheralDeviceStatus>
    {
        public override void Configure(EntityTypeBuilder<PeripheralDeviceStatus> builder)
        {
            base.Configure(builder);

            builder.HasKey(p => p.Id);

            builder.Property(p=>p.Name)
                .HasMaxLength(64);
        }
    }
}
