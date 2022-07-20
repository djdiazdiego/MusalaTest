using System.Collections.Generic;

namespace DoItFast.Domain.Core.Abstractions.Wrappers
{
    public interface IValidationResponse : IBaseResponse
    {
        /// <summary>
        /// Error dictionary.
        /// </summary>
        Dictionary<string, string> Errors { get; }
    }
}
