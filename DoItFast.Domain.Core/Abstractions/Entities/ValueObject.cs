using Force.DeepCloner;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoItFast.Domain.Core.Abstractions.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ValueObject : ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right) =>
            !(left is null ^ right is null) && (left is null || left.Equals(right));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right) =>
            !(EqualOperator(left, right));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            obj != null && obj.GetType() == GetType() &&
            this.GetEqualityComponents().SequenceEqual(((ValueObject)obj).GetEqualityComponents());

        /// <inheritdoc />
        public override int GetHashCode() =>
            GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);

        /// <inheritdoc />
        public object Clone() =>
            this.DeepClone();
    }
}
