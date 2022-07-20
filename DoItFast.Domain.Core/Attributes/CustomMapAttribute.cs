using System;

namespace DoItFast.Domain.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CustomMapAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="converter"></param>
        /// <param name="reverseMap"></param>
        public CustomMapAttribute(Type destination, Type converter, bool reverseMap = false)
        {
            Destination = destination;
            Converter = converter;
            ReverseMap = reverseMap;
        }

        /// <summary>
        /// Destination type.
        /// </summary>
        public Type Destination { get; set; }

        /// <summary>
        /// Converter type.
        /// </summary>
        public Type Converter { get; set; }

        /// <summary>
        /// Invert map.
        /// </summary>
        public bool ReverseMap { get; set; }
    }
}
