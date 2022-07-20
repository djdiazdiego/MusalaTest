using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoItFast.Infrastructure.Persistence.Configurations
{
    public abstract class BaseEnumerationConfiguration<TEnumeration> : BaseEntityConfiguration<TEnumeration>
        where TEnumeration : class, IEnumeration
    {
        public override void Configure(EntityTypeBuilder<TEnumeration> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Name);
        }
    }
}
