using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DoItFast.Infrastructure.Shared.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Add entities and configurations to DbContext.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assembly"></param>
        public static void AddEntitiesAndConfiguration(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var types = typeof(IEntity).GetConcreteTypes();

            foreach (var type in types)
                modelBuilder.Entity(type);

            types = typeof(ISharedTypeEntity).GetConcreteTypes();

            foreach (var type in types)
                modelBuilder.SharedTypeEntity(type.Name, type);

            modelBuilder.ApplyConfigurationsFromAssembly(assembly, t => t.IsClass && !t.IsAbstract);
        }
    }
}
