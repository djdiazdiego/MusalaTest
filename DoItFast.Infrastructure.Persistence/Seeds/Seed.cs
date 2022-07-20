using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Persistence.Contexts;
using DoItFast.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DoItFast.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Seed for enuneration entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Seed<TEntity> : ISeed where TEntity : class, IEnumeration
    {
        /// <inheritdoc />
        public virtual async Task SeedAsync(IServiceProvider provider, CancellationToken cancelationToken)
        {
            using var scope = provider.CreateScope();

            var enumType = typeof(TEntity).BaseType?.GetGenericArguments().First();
            if (enumType != null)
            {
                var enumValues = Enum.GetValues(enumType);

                using var context = scope.ServiceProvider.GetRequiredService<DbContextWrite>();

                foreach (Enum value in enumValues)
                {
                    var name = value.GetDescription();
                    var dbEntity = await context.Set<TEntity>().Where(p => p.Id == value).FirstOrDefaultAsync(cancelationToken);

                    if (dbEntity != null)
                    {
                        if (dbEntity.Name != name)
                        {
                            dbEntity.SetName(name);
                            context.Update(dbEntity);
                        }
                    }
                    else
                    {
                        if (Activator.CreateInstance(typeof(TEntity), new object[] { value, name }) is TEntity instance)
                        {
                            context.Add(instance);
                        }
                    }
                }

                await context.SaveChangesAsync(cancelationToken);
            }
        }
    }

}
