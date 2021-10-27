using Azure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Telstra.Twins.Exceptions;

namespace Telstra.Twins.Responses
{
    public partial class TwinsResponse
    {
        public static TwinsResponse Ok() =>
            new TwinsResponse()
            {
                Status = HttpStatusCode.OK
            };

        public static TwinsResponse<T> Ok<T>() =>
            new TwinsResponse<T>()
            {
                Status = HttpStatusCode.OK,
            };

        public static TwinsResponse<T> Ok<T>(T t) =>
            new TwinsResponse<T>()
            {
                Status = HttpStatusCode.OK,
                Data = t
            };

        public static TwinsResponse<T> Ok<T>(Response<T> r) =>
            new TwinsResponse<T>()
            {
                Status = HttpStatusCode.OK,
                Data = r.Value
            };

        public static TwinsResponse<T> NotFound<T>(string code, string msg = null) =>
            new TwinsResponse<T>()
            {
                Status = HttpStatusCode.NotFound,
                Exception = RequestFailedExceptionContent.Create(msg)
            };

        public static TwinsResponse NotFound(string msg = null) =>
            new TwinsResponse()
            {
                Status = HttpStatusCode.NotFound
            };


        public static TwinsResponse FromResponse(Response r) =>
            new TwinsResponse()
            {
                Status = (HttpStatusCode)r.Status == HttpStatusCode.NoContent ? HttpStatusCode.OK : (HttpStatusCode)r.Status
            };

        public static TwinsResponse<T> FromStringResponse<T>(Response<string> r)
        {
            var raw = r.GetRawResponse();
            var response = new TwinsResponse<T>()
            {
                Status = (HttpStatusCode)raw.Status == HttpStatusCode.NoContent ? HttpStatusCode.OK : (HttpStatusCode)raw.Status,
                Data = JsonConvert.DeserializeObject<T>(r.Value)
            };

            return response;
        }

        public static TwinsResponse Error(HttpStatusCode status, [CallerMemberName] string memberName = null)
        {
            return new TwinsResponse()
            {
                Status = status
            };
        }

        public static TwinsResponse Error(RequestFailedException e, [CallerMemberName] string memberName = null)
        {
            var content = RequestFailedExceptionContent.Create(e);
            Debug.WriteLine($"[{memberName}]: {content.error.message})");
            return new TwinsResponse()
            {
                Status = (HttpStatusCode)e.Status,
                Exception = content
            };
        }

        public static TwinsResponse<T> Error<T>(RequestFailedException e, [CallerMemberName] string memberName = null)
        {
            var content = RequestFailedExceptionContent.Create(e);
            var msg = string.Empty;
            if (content == null)
                msg = e.Message;
            else
            {
                var errors = new List<string>() { content.error.message };
                if (content.error.details != null)
                    errors.AddRange(content.error.details?.Select(d => d.message));
                msg = string.Join("\n", errors);
            }
            Debug.WriteLine($"[{memberName}]: {msg})");
            return new TwinsResponse<T>()
            {
                Status = (HttpStatusCode)e.Status,
                Exception = content
            };
        }

        public static TwinsResponse<T> Error<T>(HttpStatusCode status, string msg)
        {
            var content = RequestFailedExceptionContent.Create(msg);
            return new TwinsResponse<T>()
            {
                Status = status,
                Exception = content
            };
        }

        // NOTE - Value is dropped when casting to different type
        // intended use is for casting exceptions from TwinsReponse<string> to expected return type
        public static TwinsResponse<T> To<T>(TwinsResponse from)
        {
            return new TwinsResponse<T>()
            {
                Status = from.Status,
                Exception = from.Exception
            };
        }
        public static TwinsResponse<T> To<T>(TwinsResponse from, T value)
        {
            return new TwinsResponse<T>()
            {
                Status = from.Status,
                Exception = from.Exception,
                Data = value
            };
        }
    }
}