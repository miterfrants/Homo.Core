using System;
using System.Collections.Generic;
using System.Net;

namespace Homo.Core.Constants
{
    public class ErrorObject
    {
        public int ErrorCode;
        public string ErrorMessage;
        public ErrorObject(int ErrorCode, string ErrorMessage)
        {
            this.ErrorCode = ErrorCode;
            this.ErrorMessage = ErrorMessage;
        }
    }

    public class CustomException : Exception
    {
        public HttpStatusCode code;
        public string errorCode;
        public Dictionary<string, string> option;
        public Dictionary<string, dynamic> payload;
        public CustomException(string errorCode, HttpStatusCode code = HttpStatusCode.OK, Dictionary<string, string> option = null, Dictionary<string, dynamic> payload = null) : base()
        {
            this.code = code;
            this.errorCode = errorCode;
            this.option = option;
            this.payload = payload;
        }
    }
}
