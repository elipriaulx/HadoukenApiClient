using System;
using System.Net;

namespace HadoukenApiClient.Exceptions
{
    public class ClientException : Exception
    {
        #region  Properties

        public WebExceptionStatus? WebExceptionStatus { get; }
        public HttpStatusCode? HttpStatus { get; }

        #endregion

        #region Constructors

        internal ClientException(string message) : base(message)
        {

        }

        internal ClientException(string message, Exception innerException) : base(message, innerException)
        {
            var ex = innerException as WebException;

            WebExceptionStatus = ex?.Status;
            HttpStatus = (ex?.Response as HttpWebResponse)?.StatusCode;
        }

        #endregion
    }
}
