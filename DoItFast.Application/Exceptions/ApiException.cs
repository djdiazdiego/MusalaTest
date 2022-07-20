using System.Globalization;

namespace DoItFast.Application.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiException() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ApiException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public ApiException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
