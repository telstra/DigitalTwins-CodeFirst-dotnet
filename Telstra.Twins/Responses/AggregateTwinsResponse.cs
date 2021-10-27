using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Telstra.Twins.Responses
{
    public partial class AggregateTwinsResponse
    {
        public AggregateTwinsResponse() { }
        public AggregateTwinsResponse(TwinsResponse twinsResponse)
        {
            Responses.Add(twinsResponse);
        } 

        public List<TwinsResponse> Responses { get; } = new List<TwinsResponse>();

        public bool Success => !Responses.Any(r => r.Status != HttpStatusCode.OK);

        public HttpStatusCode FirstError => Responses.FirstOrDefault(r => r.Status != HttpStatusCode.OK).Status;
    }

    public partial class AggregateTwinsResponse<T> : AggregateTwinsResponse
    {
        public AggregateTwinsResponse() { }
        public AggregateTwinsResponse(TwinsResponse<T> twinsResponse)
        {
            Responses.Add(twinsResponse);
        }

        public new List<TwinsResponse<T>> Responses { get; } = new List<TwinsResponse<T>>();
    }
}