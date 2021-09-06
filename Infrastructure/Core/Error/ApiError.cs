using Infrastructure.Error;
using Models.Core.Error;

namespace Infrastructure.Core.Error
{
    public class ApiError : IError
    {
        /// <summary>
        /// Error text
        /// </summary>
        public string Error { get; }
        /// <summary>
        /// Field with error
        /// </summary>
        public string Field { get; }
        /// <summary>
        /// Error code (const for client)
        /// </summary>
        public ErrorType? ErrorCode { get; }
        public int? ErrorCodeId => (int?) ErrorCode;

        public ApiError(ErrorType? errorCode, string field = null, string error = null)
        {
            Field = field;
            ErrorCode = errorCode;
            Error = error;
        }
    }
}