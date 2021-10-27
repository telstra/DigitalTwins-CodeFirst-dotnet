using Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Telstra.Twins.Exceptions
{
    [Serializable]
    public class RequestFailedExceptionContent
    {
        public static RequestFailedExceptionContent Create(RequestFailedException e)
        {
            var msg = e.Message;
            msg = msg.Substring(msg.IndexOf("Content:") + "Content:".Length).Trim();
            msg = msg.Substring(0, msg.IndexOf("\r\n"));
            try
            {
                var result = JsonConvert.DeserializeObject<RequestFailedExceptionContent>(msg);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }

        public static RequestFailedExceptionContent Create(string msg) =>
            new RequestFailedExceptionContent() { error = new Error() { message = msg } };

        public Error error { get; set; }
    }

    [Serializable]
    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<Error> details { get; set; }
    }
}
