using System;
using System.Net;
using Telstra.Twins.Exceptions;

namespace Telstra.Twins.Responses
{
    [Serializable]
    public partial class TwinsResponse
    {
        public HttpStatusCode Status { get; set; }

        public string ClientRequestId { get; set; }

        public RequestFailedExceptionContent Exception { get; set; }
    }

    [Serializable]
    public partial class TwinsResponse<T> : TwinsResponse
    {
        public T Data { get; set; }
    }
}