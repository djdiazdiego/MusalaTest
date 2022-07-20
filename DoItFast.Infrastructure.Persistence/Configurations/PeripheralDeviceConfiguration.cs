using DoItFast.Domain.Models.GatewayAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoItFast.Infrastructure.Persistence.Configurations
{
    public class PeripheralDeviceConfiguration : BaseEntityConfiguration<PeripheralDevice>
    {
        public override void Configure(EntityTypeBuilder<PeripheralDevice> builder)
        {
            base.Configure(builder);

            builder.HasKey(p => p.Id);

            builder.Property(p=> p.Vendor)
                .HasMaxLength(64);

            builder.HasOne(p => p.PeripheralDeviceStatus)
                .WithMany()
                .HasForeignKey(p => p.PeripheralDeviceStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
