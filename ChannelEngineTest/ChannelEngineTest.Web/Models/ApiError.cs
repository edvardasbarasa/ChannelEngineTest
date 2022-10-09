using System.Collections.Generic;

namespace ChannelEngineTest.Web.Models
{
    public class ApiError
    {
        public string Message { get; }

        public string ErrorCode { get; }

        /// <summary>
        /// Additional information
        /// </summary>
        public IDictionary<string, string[]> Arguments { get; }

        public ApiError(string message, string errorCode, IDictionary<string, string[]> arguments = null)
        {
            Message = message;
            ErrorCode = errorCode;
            Arguments = arguments;
        }
    }
}