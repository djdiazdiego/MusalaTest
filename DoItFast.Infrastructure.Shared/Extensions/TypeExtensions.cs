using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DoItFast.Infrastructure.Shared.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Get the concrete types that implement or inherit from a given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetConcreteTypes(this Type type, Assembly? assembly = null)
        {
            var assemblyTypes = assembly != null ? assembly.GetTypes() : type.Assembly.GetTypes();
            var types = !(type.IsGenericType && type.IsTypeDefinition) ?
                assemblyTypes.Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(type)) :
                assemblyTypes.Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type));

            return types.ToArray();
        }

        /// <summary>
        /// Get concrete type specific filter.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filter"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type GetConcreteTypeWithFilter(this Type type, Expression<Func<Type, bool>> filter = null, Assembly? assembly = null)
        {
            var assemblyTypes = assembly != null ? assembly.GetTypes() : type.Assembly.GetTypes();
            var query = assemblyTypes.Where(t => t.IsClass && !t.IsAbstract && !t.IsInterface && t.IsAssignableTo(type))
                .AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            return query.First();
        }
    }
}
