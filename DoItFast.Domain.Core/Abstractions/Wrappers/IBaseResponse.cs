namespace DoItFast.Domain.Core.Abstractions.Wrappers
{
    public interface IBaseResponse
    {
        /// <summary>
        /// Status code
        /// </summary>
        int Code { get; }

        /// <summary>
        /// Response status
        /// </summary>
        bool Succeeded { get; }

        /// <summary>
        /// Custom message
        /// </summary>
        string Message { get; }
    }
}
