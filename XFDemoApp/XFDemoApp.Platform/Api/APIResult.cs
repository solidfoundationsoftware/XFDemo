using System;
using System.Collections.Generic;
using System.Text;

namespace XFDemoApp.Platform.Api
{
    public class APIResult
    {
        public bool Success { get; set; }
        public string FailureMessage { get; set; }
        public Exception FailureException { get; set; }

        public APIResult() { }
        public APIResult(bool success) : this(success, "") { }
        public APIResult(bool success, string failureMessage) : this(success, failureMessage, null) { }
        public APIResult(bool success, Exception failureException) : this(success, failureException?.Message, failureException) { }
        public APIResult(bool success, string failureMessage, Exception failureException)
        {
            Success = success;
            FailureMessage = string.IsNullOrEmpty(failureMessage) ? "Operation Failed" : failureMessage;
            FailureException = failureException ?? new Exception(failureMessage);
        }
    }
}
