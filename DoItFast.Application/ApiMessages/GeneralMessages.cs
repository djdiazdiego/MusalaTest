using System.ComponentModel;

namespace DoItFast.Application.ApiMessages
{
    public enum GeneralMessages
    {
        /// <summary>
        /// The field cannot be empty.
        /// </summary>
        [Description("The field cannot be empty")]
        NotEmpty = 1,
        /// <summary>
        /// The value is invalid.
        /// </summary>
        [Description("The value is invalid")]
        InvalidValue = 2,
        /// <summary>
        /// Must be greater.
        /// </summary>
        [Description("Must be greater than")]
        GreaterThan = 3,
        /// <summary>
        /// Must be less.
        /// </summary>
        [Description("Must be less than")]
        LessThan = 4,
        /// <summary>
        /// Length must be equal.
        /// </summary>
        [Description("Length must be equal to")]
        LengthEqual = 5,
        /// <summary>
        /// Not found.
        /// </summary>
        [Description("Not found")]
        NotFound = 6,
        /// <summary>
        /// Field cannot be null.
        /// </summary>
        [Description("The field cannot be null")]
        NotNull = 7,
        /// <summary>
        /// Maximum allowed length.
        /// </summary>
        [Description("The maximum allowed length is")]
        MaximumLength = 8,
        /// <summary>
        /// Already exists.
        /// </summary>
        [Description("Already exists")]
        AlreadyExists = 9,
        /// <summary>
        /// Invalid ip address.
        /// </summary>
        [Description("Invalid ip address")]
        InvalidIpAddress4 = 10
    }
}
