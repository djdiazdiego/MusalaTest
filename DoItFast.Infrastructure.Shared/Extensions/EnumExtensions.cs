using System.ComponentModel;

namespace DoItFast.Infrastructure.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttributeOfType<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        public static string GetDescription(this Enum value)
        {
            var attribute = value.GetAttributeOfType<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
