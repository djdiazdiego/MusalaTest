using DoItFast.Application.Exceptions;
using DoItFast.Domain.Core.Abstractions.Wrappers;

namespace DoItFast.Application.Wrappers
{
    /// <summary>
    /// Blueberry validation response
    /// </summary>
    public class ValidationResponse : IValidationResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ValidationResponse(int code, string message, ValidationException exception)
        {
            Code = code;
            Succeeded = false;
            Message = message;
            Errors = exception.Errors;
        }

        /// <inheritdoc />
        public int Code { get; set; }

        /// <inheritdoc />
        public bool Succeeded { get; set; }

        /// <inheritdoc />
        public string Message { get; set; }

        /// <inheritdoc />
        public Dictionary<string, string> Errors { get; set; }
    }
}
