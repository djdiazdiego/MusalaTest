using DoItFast.Domain.Models.GatewayAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoItFast.Infrastructure.Persistence.Configurations
{
    public class GatewayConfiguration : BaseEntityConfiguration<Gateway>
    {
        public override void Configure(EntityTypeBuilder<Gateway> builder)
        {
            base.Configure(builder);

            builder.HasKey(p => p.Id).HasName(nameof(Gateway.SerialNumber));

            builder.Property(p => p.Id)
                .HasColumnName(nameof(Gateway.SerialNumber))
                .HasMaxLength(32);
            builder.Property(p => p.ReadableName)
                .HasMaxLength(64);
            builder.Property(p => p.IpAddress)
                .HasMaxLength(16);

            builder.HasMany(p => p.PeripheralDevices)
                .WithOne(p => p.Gateway)
                .HasForeignKey(p => p.GatewayId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
