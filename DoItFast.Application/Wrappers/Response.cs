using DoItFast.Domain.Core.Abstractions.Wrappers;

namespace DoItFast.Application.Wrappers
{
    /// <summary>
    /// Blueberry response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> : IResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public Response()
        {
        }
        /// <summary>
        /// Successful response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public Response(T data, string message = null)
        {
            Code = 200;
            Succeeded = true;
            Message = message;
            Data = data;
        }
        /// <summary>
        /// Unsuccessful response
        /// </summary>
        /// <param name="message"></param>
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }

        /// <inheritdoc />
        public int Code { get; set; }

        /// <inheritdoc />
        public bool Succeeded { get; set; }

        /// <inheritdoc />
        public string Message { get; set; }

        /// <inheritdoc />
        public List<string> Errors { get; set; }

        /// <summary>
        /// Response data
        /// </summary>
        public T Data { get; set; }

        /// <inheritdoc />
        object IResponse.Data { get => Data; }
    }
}
