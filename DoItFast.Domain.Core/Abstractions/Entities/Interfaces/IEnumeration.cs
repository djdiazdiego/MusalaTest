using System;

namespace DoItFast.Domain.Core.Abstractions.Entities.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEnumeration : IEntity, IComparable
    {
        /// <summary>
        /// Name of the field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Set name
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name);
    }
}
