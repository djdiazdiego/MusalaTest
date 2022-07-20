using System;

namespace DoItFast.Domain.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FullMapAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="reverseMap"></param>
        public FullMapAttribute(Type destination, bool reverseMap = false)
        {
            Destination = destination;
            ReverseMap = reverseMap;
        }

        /// <summary>
        /// Destination type.
        /// </summary>
        public Type Destination { get; set; }

        /// <summary>
        /// Invert map.
        /// </summary>
        public bool ReverseMap { get; set; }
    }
}
