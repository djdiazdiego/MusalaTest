using AutoMapper;
using DoItFast.Domain.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DoItFast.Application.Extensions
{
    public static class MappingExtensions
    {
        /// <summary>
        /// Add auto mapper.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mapAssemblies"></param>
        public static void AddMapperExtension(this IServiceCollection services, Assembly[] mapAssemblies)
        {
            var addCustomMap = typeof(MappingExtensions)
                .GetMethod(nameof(AddCustomMap), BindingFlags.NonPublic | BindingFlags.Static);

            var addFullMap = typeof(MappingExtensions)
                .GetMethod(nameof(AddFullMap), BindingFlags.NonPublic | BindingFlags.Static);

            var valuesCustom = FindValuesToCustomMap(mapAssemblies);
            var valuesFull = FindValuesToFullMap(mapAssemblies);

            services.AddAutoMapper(config =>
            {
                foreach (var methodInfo in from value in valuesCustom
                                           let v1 = value.Item4 ? value.Item2 : value.Item1
                                           let v2 = value.Item4 ? value.Item1 : value.Item2
                                           let methodInfo = addCustomMap?.MakeGenericMethod(v1, v2, value.Item3)
                                           select methodInfo)
                {
                    methodInfo.Invoke(null, new object[] { config });
                }
                foreach (var methodInfo in from value in valuesFull
                                           let v1 = value.Item3 ? value.Item2 : value.Item1
                                           let v2 = value.Item3 ? value.Item1 : value.Item2
                                           let methodInfo = addFullMap?.MakeGenericMethod(v1, v2)
                                           select methodInfo)
                {
                    methodInfo.Invoke(null, new object[] { config });
                }

            }, mapAssemblies);
        }

        /// <summary>
        /// Create custom map
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TConverter"></typeparam>
        /// <param name="config"></param>
        private static void AddCustomMap<TSource, TDestination, TConverter>(IMapperConfigurationExpression config)
            where TSource : class
            where TDestination : class
            where TConverter : class, ITypeConverter<TSource, TDestination> =>
                config.CreateMap<TSource, TDestination>()
                    .ConstructUsingServiceLocator()
                    .ConvertUsing<TConverter>();

        /// <summary>
        /// Create custom map
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="config"></param>
        private static void AddFullMap<TSource, TDestination>(IMapperConfigurationExpression config)
            where TSource : class
            where TDestination : class, new() =>
                config.CreateMap<TSource, TDestination>()
                    .ConstructUsing(_ => new TDestination());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static IEnumerable<(Type, Type, Type, bool)> FindValuesToCustomMap(IEnumerable<Assembly> assemblies)
        {
            var result = new List<(Type, Type, Type, bool)>();
            var typeInfos = FindWithDuplicateAttributeInAssemblies<CustomMapAttribute>(assemblies);

            result.AddRange(from item in typeInfos
                            let attributes = item.GetCustomAttributes<CustomMapAttribute>()
                            from attribute in attributes
                            select (item.AsType(), attribute.Destination, attribute.Converter, attribute.ReverseMap));

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static IEnumerable<(Type, Type, bool)> FindValuesToFullMap(IEnumerable<Assembly> assemblies)
        {
            var result = new List<(Type, Type, bool)>();
            var typeInfos = FindWithDuplicateAttributeInAssemblies<FullMapAttribute>(assemblies);

            result.AddRange(from item in typeInfos
                            let attributes = item.GetCustomAttributes<FullMapAttribute>()
                            from attribute in attributes
                            select (item.AsType(), attribute.Destination, attribute.ReverseMap));

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static IEnumerable<TypeInfo> FindWithDuplicateAttributeInAssemblies<TAttribute>(IEnumerable<Assembly> assemblies) where TAttribute : Attribute
        {
            return (from assembly in assemblies
                    where !assembly.IsDynamic
                    from subAssembly in assembly.DefinedTypes
                    where (subAssembly.IsPublic || subAssembly.IsNestedPublic) && subAssembly.GetCustomAttributes<TAttribute>().Any()
                    from attribute in subAssembly.GetCustomAttributes<TAttribute>()
                    select subAssembly).Distinct().ToList();
        }
    }
}
